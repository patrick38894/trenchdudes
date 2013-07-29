using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		print("you clicked the test object");
		Transform next = Instantiate(this, transform.position + new Vector3(2.0f,0,0), Quaternion.identity) as Transform;
	}
}
