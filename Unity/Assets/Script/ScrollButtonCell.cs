using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScrollButtonCell : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickButton()
	{
		SceneManager.LoadScene ("Start");
	}
}
