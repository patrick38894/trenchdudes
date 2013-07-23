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


	// Use this for initialization
	void awake() {
	}
	void Start () {
		bgRect = new Rect (0, Screen.height/2, Screen.width, Screen.height/2);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		//TODO scaling
		int buttonPosX = 128;
		int buttonPosY = Screen.height - 128 -64;
		GUI.skin = skin;
		int buttonOffset = 128 + 128;

		//this should go into "on window resize" if that is a real thing;
		GUI.DrawTexture (bgRect, background);

		if (attackToggle) {
			if (GUI.Button(new Rect(buttonPosX, buttonPosY, 128, 128), attack)) {
				//do something
				IOManager.processOrder("attack");
			}
			buttonPosX += buttonOffset;
		}
		if (moveToggle) {
			if (GUI.Button(new Rect(buttonPosX, buttonPosY, 128, 128), move)) {
				//do something
				IOManager.processOrder("move");
			}
			buttonPosX += buttonOffset;
		}
		if (digToggle) {
			if (GUI.Button(new Rect(buttonPosX, buttonPosY, 128, 128), dig)) {
				//do something
				IOManager.processOrder("dig");
			}
			buttonPosX += buttonOffset;
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
