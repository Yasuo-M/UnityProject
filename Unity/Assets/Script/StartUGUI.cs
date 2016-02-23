using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUGUI : MonoBehaviour {

	[SerializeField]
	private Image _image_obj;

	// Use this for initialization
	void Start () {
		AssetBundleLoadManager _bundle = new AssetBundleLoadManager ();
		_bundle.LoadStart ("http://192.168.0.93:80/Android/cube.unity3d", "cube", (obj)=>{
			Debug.Log("obj===:"+obj.name);
			Instantiate(obj);
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
