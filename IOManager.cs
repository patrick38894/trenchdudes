using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IOManager : MonoBehaviour {


	public Texture2D selectionHighlight; //texture for highlight box
	protected static string [] unaryOrders = {"hold position", "commit sudoku", "dig"};
	protected static Rect selectionBox = new Rect(0,0,0,0); //the highlight box
	protected static List<Unit> allUnits; //list of all units
	protected static List<Unit> selectedUnits; //list of selected units
	protected static bool selectionLock = false;
	protected static bool awaitingOrders = false;
	protected static string currentInstruction = "zilch" ; //units will take a string as an order
	protected static int HUDONYAxis; //not a real const because of window resizing - the line where the HUD meets the viewport
	protected static Vector3 startClick = - Vector3.one; //the start of the selectionbox -vector3.one is the null
	

	protected HUD hud; //this guy processes all the button stuff

	// Use this for initialization
	void Start () {
		hud = GetComponent<HUD>();
		selectedUnits = new List<Unit>();
		allUnits = new List<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		HUDONYAxis = Screen.height / 4;
		if (Input.GetMouseButtonDown(0)) {
			if (awaitingOrders) { //find the target and send it as an arg to the selected units
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
					sendOrder(hit.transform/*.gameObject*/);


			}
			else if (!selectionLock) {
			//do selcetion box stuff
				if (Input.mousePosition.y > HUDONYAxis)
					startClick = Input.mousePosition;
			}
		}
		else if (Input.GetMouseButton(0) && startClick != -Vector3.one) {
			selectionBox = new Rect(startClick.x, invert(startClick.y), Input.mousePosition.x - startClick.x, invert(Input.mousePosition.y) - invert(startClick.y));
			if (selectionBox.width < 0) {
				selectionBox.x += selectionBox.width;
				selectionBox.width = - selectionBox.width;
			}
			if (selectionBox.height < 0) {
				selectionBox.y += selectionBox.height;
				selectionBox.height = - selectionBox.height;
			}
			if (selectionBox.height + selectionBox.y > invert(HUDONYAxis))
				selectionBox.height = invert(HUDONYAxis) - invert(startClick.y);
		}
		else if (Input.GetMouseButtonUp(0)) {
			startClick = -Vector3.one;
			updateSelection();
		}
		//color the units
		foreach (Unit dude in allUnits)
			dude.renderer.material.color = Color.blue;
		foreach (Unit dude in selectedUnits)
			dude.renderer.material.color = Color.red;
		
	}

	void OnGUI() {
		if (startClick != -Vector3.one) {
			GUI.color = new Color(1,1,1, 0.5f);
			GUI.DrawTexture(selectionBox, selectionHighlight);
		}
	}

	public static void initiateUnit(Unit newUnit) { //call this from Unit::Start()
		allUnits.Add(newUnit);
	}

	protected void updateSelection() {
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			selectedUnits.Clear();
		foreach (Unit dude in allUnits) {
			Vector3 camPos = Camera.mainCamera.WorldToScreenPoint(dude.transform.position);
			camPos.y = invert(camPos.y);
			if (selectionBox.Contains(camPos))
				selectedUnits.Add(dude);
		}
		if (selectedUnits.Count == 0)
			hud.clearHUD();
		else {
			Unit dominant = selectedUnits[0];
			foreach (Unit dude in selectedUnits) {
				if (dude.priority > dominant.priority)
					dominant = dude;
			}
			hud.setHUD(dominant);
		}

	}

	public static void processOrder (string order) {
		currentInstruction = order;
		for (int i = 0; i < unaryOrders.Length; ++i) {
			if (unaryOrders[i] == order) {
				sendOrder();
				return;
			}
		}
		selectionLock = true; //lock selection to allow player to click the target of the order
		awaitingOrders = true;
	}

	protected static void sendOrder (Transform target) {
		foreach (Unit dude in selectedUnits) {
			//dude.recieveOrder(currentInstruction, target);
		}
		selectionLock = false;
		awaitingOrders = false;
		currentInstruction = "zilch";
	}

	protected static void sendOrder () { //for unary orders
		foreach (Unit dude in selectedUnits) {
			//dude.recieveOrder(currentInstruction);
		}
	}

	public static float invert(float why) {
		return Screen.height - why;
	}
}
