using UnityEngine;
using System.Collections;

class ActionNode {
	public Vector3? position = null;
	public MonoBehaviour target = null;
	public string action = null;
	public ActionNode next = null;

	public ActionNode(Vector3 data, string funcname) {
		position = data;
		action = funcname;
	}

	public ActionNode(ref MonoBehaviour data, string funcname) {
		target = data;
		action = funcname;
	}
}
