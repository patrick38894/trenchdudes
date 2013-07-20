using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public int priority = 5;
	public bool selected = false; //do not use
	public bool moveToggle = false;
	public bool attackToggle = false;
	public bool digToggle = false;
	protected bool initiated = false;

	// Use this for initialization
	void Start () {
		IOManager.initiateUnit(this);
	}
	
	// Update is called once per frame
	void Update () {
	} 
} 
