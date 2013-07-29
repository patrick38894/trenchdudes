using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public Texture attack;
	public Texture move;
	public Texture dig;
	public Texture background;
	public GUISkin skin;

	protected bool attackToggle = false;
	protected bool moveToggle = false;
	protected bool digToggle = false;
	protected Rect bgRect = new Rect (0,0,0,0);
	protected Rect buttonRect = new Rect (0,0,0,0);
	protected int buttonSize;
	protected int buttonPosY;
	protected int buttonPosX;
	protected int buttonOffset;


	// Use this for initialization
	void awake() {
	}
	void Start () {
		bgRect = new Rect (0, Screen.height *3/4, Screen.width, Screen.height/4);
		buttonPosX = Screen.width / 18;
		buttonPosY = Screen.height * 26/32;
		buttonOffset = Screen.width /9;
		buttonSize = Screen.width /14;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		
		GUI.skin = skin;
		GUI.DrawTexture (bgRect, background);
		int temp = buttonPosX;

		if (attackToggle) {
			if (GUI.Button(new Rect(temp, buttonPosY, buttonSize, buttonSize), attack)) {
				//do something
				IOManager.processOrder("attack");
			}
			temp += buttonOffset;
		}
		if (moveToggle) {
			if (GUI.Button(new Rect(temp, buttonPosY, buttonSize, buttonSize), move)) {
				//do something
				IOManager.processOrder("move");
			}
			temp += buttonOffset;
		}
		if (digToggle) {
			if (GUI.Button(new Rect(temp, buttonPosY, buttonSize, buttonSize), dig)) {
				//do something
				IOManager.processOrder("dig");
			}
			temp += buttonOffset;
		}

	}



	public void setHUD (Unit currentUnit) {
		attackToggle = currentUnit.attackToggle;
		moveToggle = currentUnit.moveToggle;
		digToggle = currentUnit.digToggle;	
	}
		
	public void clearHUD() {
		attackToggle = false;
		moveToggle = false;
		digToggle = false;	
	}
}
