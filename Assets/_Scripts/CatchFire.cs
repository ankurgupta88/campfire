using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider col)
	{
		if (col.CompareTag("Hot"))
		{
			gameObject.SetActive (true);

		}                     
	}
}
