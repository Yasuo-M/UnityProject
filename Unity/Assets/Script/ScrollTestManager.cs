using UnityEngine;
using System.Collections;

public class ScrollTestManager : MonoBehaviour {

	[SerializeField]
	private GameObject _scroll_content;
	[SerializeField]
	private GameObject _list;


	// Use this for initialization
	void Start () {
		for (int i = 0; i < 30; i++) {
			GameObject obj = Instantiate (_list) as GameObject;
			obj.transform.SetParent (_scroll_content.transform, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
