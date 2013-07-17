using UnityEngine;
using System.Collections;

class ActionQueue {

	protected ActionNode head = null;
	
	public void push(Vector3 position, string funcname) {
		ActionNode temp = new ActionNode(position, funcname);
		temp.next = head;
		head = temp;
	}

	public void push(ref MonoBehaviour target, string funcname) {
		ActionNode temp = new ActionNode(ref target, funcname);
		temp.next = head;
		head = temp;
	}

	public void peek (ref Vector3? position, ref MonoBehaviour target, ref string funcname) {
		if (head == null) {
			position = null;
			target = null;
			return;
		}
		position = head.position;
		target = head.target;
		funcname = head.action;
	}

	public void pop (ref Vector3? position, ref MonoBehaviour target, ref string funcname) {
		peek(ref position, ref target, ref funcname);
		if (head != null)
			head = head.next;
	}
}


