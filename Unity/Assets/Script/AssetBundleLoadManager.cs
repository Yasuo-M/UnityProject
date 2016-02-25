using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AssetBundleLoadManager : MonoBehaviour
{
	private List<LoadTask> _load_tasks = new List<LoadTask>();
	private bool _is_load_start = false;

	class LoadTask
	{
		public string bundleUrl;
		public string assetName;
		public int version = 1;
		public Action<UnityEngine.Object> successCbk;
	}

	public static AssetBundleLoadManager LoadStart(string bundle_url, string asset_name, Action<UnityEngine.Object> cbk)
	{
		AssetBundleLoadManager script;
		GameObject obj = GameObject.Find ("AssetBundleLoadManager");
		if (obj == null) {
			obj = new GameObject ("AssetBundleLoadManager");
			script = obj.AddComponent<AssetBundleLoadManager> ();
		}
		else
			script = obj.GetComponent<AssetBundleLoadManager> ();
		script.init (bundle_url, asset_name, cbk);
		return script;
	}

	private void init(string bundle_url, string asset_name, Action<UnityEngine.Object> cbk)
	{
		LoadTask load_task = new LoadTask ();
		load_task.bundleUrl = bundle_url;
		load_task.assetName = asset_name;
		load_task.successCbk = cbk;
		_load_tasks.Add (load_task);
	}

	void Start()
	{
		StartCoroutine (main_loop());
	}

	IEnumerator main_loop()
	{
		while (true) {
			while (_load_tasks.Count == 0 || _is_load_start)
				yield return null;
			_is_load_start = true;
			StartCoroutine (DownloadAndCache ());
		}
	}

	IEnumerator DownloadAndCache ()
	{
		//キャッシュの準備が出来るまで待機
		while (!Caching.ready)
			yield return null;

		//キャッシュに無ければダウンロード
		using (WWW www = WWW.LoadFromCacheOrDownload (_load_tasks[0].bundleUrl, _load_tasks[0].version)) {
			yield return www;
			//エラーがあれば
			if (www.error != null) {
				throw new UnityException ("WWW download had an error" + www.error);
			}
			AssetBundle bundle = www.assetBundle;
			_load_tasks[0].successCbk(bundle);
			//圧縮あ終わったらAssetBundlesをアンロード
			bundle.Unload (false);
			_is_load_start = false;
			_load_tasks.RemoveAt(0);
		}
		//www.Disposeが強制的に呼ばれる
	}
}
