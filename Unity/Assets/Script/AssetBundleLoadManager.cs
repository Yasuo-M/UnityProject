using UnityEngine;
using System.Collections;
using System;

public class AssetBundleLoadManager : MonoBehaviour
{
	private string _bundle_url;
	private string _asset_name;
	private int version = 1;
	private Action<UnityEngine.Object> _success_cbk;

	public AssetBundleLoadManager LoadStart(string bundle_url, string asset_name, Action<UnityEngine.Object> cbk)
	{
		GameObject obj = new GameObject ("AssetBundleLoadManager");
		AssetBundleLoadManager script = obj.AddComponent<AssetBundleLoadManager> ();
		script._bundle_url = bundle_url;
		script._asset_name = asset_name;
		script._success_cbk = cbk;
		return script;
	}

	void Start ()
	{
		StartCoroutine (DownloadAndCache());
	
	}

	IEnumerator DownloadAndCache ()
	{
		//キャッシュの準備が出来るまで待機
		while (!Caching.ready)
			yield return null;

		//キャッシュに無ければダウンロード
		using (WWW www = WWW.LoadFromCacheOrDownload (_bundle_url, version)) {
			yield return www;
			//エラーがあれば
			if (www.error != null) {
				throw new UnityException ("WWW download had an error" + www.error);
			}
			AssetBundle bundle = www.assetBundle;
			if (_asset_name == "") {
				_success_cbk (bundle.mainAsset);
			} else {
				_success_cbk (bundle.LoadAsset<GameObject> (_asset_name));
			}
			//圧縮あ終わったらAssetBundlesをアンロード
			bundle.Unload (false);
		}
		//メモリーがwebstreamから解放(www.Disposeが強制的に呼ばれる)
	}
}
