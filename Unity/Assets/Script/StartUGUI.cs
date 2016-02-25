using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUGUI : MonoBehaviour {

	[SerializeField]
	private Image _image_obj;
	[SerializeField]
	private Image _image_obj2;
	[SerializeField]
	private Image _image_obj3;

	// Use this for initialization
	void Start () {
		AssetBundleLoadManager.LoadStart ("http://192.168.0.101:80/Android/chara.unity3d", "", (obj)=>{
			AssetBundle bundle = obj as AssetBundle;
			Debug.Log("obj===:"+obj.name);
			Instantiate(bundle.LoadAsset("aaaa"));
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickScrollTest()
	{
		SceneManager.LoadScene ("UguiScrollTest");
	}

	public void OnClickCleanCache()
	{
		Caching.CleanCache();
	}
}
