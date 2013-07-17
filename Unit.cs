using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public bool selected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.isVisible && Input.GetMouseButton(0)) {
			Vector3 camPos = Camera.mainCamera.WorldToScreenPoint(transform.position);
			camPos.y = CameraOperator.InvertMouseY(camPos.y);
			selected = CameraOperator.selection.Contains(camPos);
		}
		if (selected)
			renderer.material.color = Color.red;
		else
			renderer.material.color = Color.white;
	}
}
