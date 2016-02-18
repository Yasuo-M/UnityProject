using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartUGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickScrollTest()
	{
		SceneManager.LoadScene ("UguiScrollTest");
	}
}
