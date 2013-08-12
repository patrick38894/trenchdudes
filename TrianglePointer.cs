using UnityEngine;
using System.Collections;

public class TrianglePointer : MonoBehaviour {
	public Triangle triangle;
	
	public void OnMouseDown() {
		Debug.Log ("doing debug stuff");
		//renderer.material.color = Color.red;
		if (triangle.parent != null)
			triangle.parent.transform.renderer.material.color = Color.red;
		
	}
	
}
