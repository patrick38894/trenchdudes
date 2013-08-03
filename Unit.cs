using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public int speed = 5;
	public float stopDistanceOffset = 2.0f;
	public int priority = 5;
	public bool selected = false; //do not use
	public bool moveToggle = false;
	public bool attackToggle = false;
	public bool digToggle = false;
	protected bool initiated = false;
	protected ActionQueue actionQueue = new ActionQueue();
	public Triangle currentPosition;

	// Use this for initialization
	void Start () {
		IOManager.initiateUnit(this);
	}
	
	// Update is called once per frame
	void Update () {
		updateAction();
	} 


	protected void updateMove(Vector3 destination) {
		Vector3 direction = (destination - transform.position);
		direction.y = 0;
		transform.rigidbody.velocity = direction.normalized * speed;
		Vector3 tempVec = transform.position;
		tempVec.y = 0;
		if (Vector3.Distance(tempVec, destination) < stopDistanceOffset) {
			print ("order complete");
			string tempStr;
			Transform tempTr;
			actionQueue.peek (out tempStr, out tempTr);
			TrianglePointer tempTriPtr;
			tempTriPtr = tempTr.GetComponent<TrianglePointer>();
			if (tempTriPtr != null)
				currentPosition = tempTriPtr.triangle;
			else {
				Unit TempUnit;
				TempUnit = tempTr.GetComponent<Unit>();
				if (TempUnit != null)
					currentPosition = TempUnit.currentPosition;
			}
			actionQueue.dequeue();
		}
	}

	public void recieveOrder(string order, Transform target) {
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			actionQueue.nuke();
		actionQueue.enqueue(order, target);
	}

	protected void updateAction() {
		if (actionQueue.isEmpty()) 
			return;

		Transform target;
		string funcName;
		actionQueue.peek(out funcName, out target);
		//TODO use boolean flags to disable options
		switch(funcName) {
		case "move": {
			     updateMove(target.position);
			     break;
		}
		case "dig": {
			    print("I am digging a hole now");
			    break;
		}
		case "attack": {
			       print("attacking the enemy");
			       break;
		}
		default:
			print ("this unit cannot " + funcName);
			break;
		}
	}
} 
