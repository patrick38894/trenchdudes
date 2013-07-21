using UnityEngine;
using System.Collections;

public class ActionQueue {

	protected class ActionNode {
		public Transform target;
		public string funcname;
		public ActionNode next;
		public ActionNode previous = null;

		public ActionNode(string s, Transform t, ActionNode n) {
			funcname = s;
			target = t;
			next = n;
		}
	}

	protected ActionNode head = null;
	protected ActionNode tail = null;

	public bool isEmpty() {
		
		if (head == null)
			return true;
		return false;
	}

	public void nuke () {
		head = tail = null;
	}


	public void enqueue(string funcname, Transform target) {
		ActionNode temp = new ActionNode (funcname, target, tail);
		if (tail != null)
			tail.previous = temp;
		tail = temp;
		if (head == null)
			head = tail;
	}

	public void peek(out string funcname, out Transform target) {
		if (head == null) {
			funcname = null;
			target = null;
			return;
		}
		funcname = head.funcname;
		target = head.target;
	}
	public void dequeue() {
		if (head.previous != null) {
			head = head.previous;
			head.next = null;
			return;
		}
		head = tail = null;
	}
}
