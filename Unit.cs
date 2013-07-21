using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public int speed = 1;
	public float stopDistanceOffset = 2f;
	public int priority = 5;
	public bool selected = false; //do not use
	public bool moveToggle = false;
	public bool attackToggle = false;
	public bool digToggle = false;
	protected bool initiated = false;
	protected ActionQueue actionQueue = new ActionQueue();

	// Use this for initialization
	void Start () {
		IOManager.initiateUnit(this);
	}
	
	// Update is called once per frame
	void Update () {
		updateAction();
	} 


	protected void updateMove(Vector3 destination) {
		Vector3 direction = (destination - transform.position).normalized;
		direction.y = 0;
		transform.rigidbody.velocity = direction * speed;
		if (Vector3.Distance(transform.position, destination) < stopDistanceOffset)
			actionQueue.dequeue();
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
