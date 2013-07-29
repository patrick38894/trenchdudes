using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
class Hexagon : MonoBehaviour {

	public Triangle [] triangles = new Triangle[6];
	public static GameObject triangleMesh;
	public static float diamond = 1f / 2f * (float) Math.Sqrt(3);
	public static float hourglass = 2f / (float) Math.Sqrt(3);
	public static float sideLength = 1.0f;
	public static float sqrt3 = (float) Math.Sqrt(3);
	protected static Hexagon [] hexarray;
	public static int xdim = 8;
	public static int ydim = 8;
	protected static bool begun = false;
	protected static Hexagon buffer;
	
	public int clock6(int toClock) {
		if (toClock >= 0)
			return toClock % 6;
		return 6 + toClock; //only works for numbers >= -6
	}

	public int clock6Invert(int toInvert) {
		return clock6(toInvert + 3);
	}

	public void linkTo(int index, Hexagon targetHex) {
		Triangle.doubleLink(triangles[index], targetHex.triangles[clock6Invert(index)], diamond); //mid2mid
		Triangle.doubleLink(triangles[clock6(index-1)], 
						targetHex.triangles[clock6(clock6Invert(index) +1)], hourglass); //left2left
		Triangle.doubleLink(triangles[clock6(index+1)], 
						targetHex.triangles[clock6(clock6Invert(index) -1)], hourglass); //left2left
	}

	public void linkIn () {
		for (int i = 0; i < 6; ++i) {
			Triangle.doubleLink(triangles[i], triangles[clock6(i-1)], diamond);
			Triangle.doubleLink(triangles[i], triangles[clock6(i+1)], diamond);
			Triangle.doubleLink(triangles[i], triangles[clock6(i+3)], hourglass);
		}
	}

	public void initWrapper() {

		hexarray = new Hexagon[xdim];
		hexarray[0] = buffer = GetComponent<Hexagon>();
		init(0, 0);
	}
//	public void OnMouseDown() {
//		print("clicked");
//		Vector3 position = transform.position;
//		position.x += sqrt3 * sideLength; 
//		GameObject nextHex = Instantiate(this, position, Quaternion.identity) as GameObject;
//	}

	public void init (int x, int y) {
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		bool isEven = (y%2 ==0);
		if (isEven &&  y!=0) {//link upwards leaning to the right
			linkTo(0,hexarray[x]);
			if (x > 0)
				linkTo(5,hexarray[x-1]);
		}
		else if (! isEven) {
			linkTo(5,hexarray[x]);
			if (x < xdim -1) 
				linkTo(0,hexarray[x+1]);
		}
		
		if (y == ydim-1)
			if ((isEven && x == xdim-1) || (!isEven && x == 0))
				return;
		if (isEven) {
			if (x == xdim-1)
				position.x += sqrt3 / 2.0f * sideLength;
			else
				position.x += sqrt3 * sideLength;
		}
		else {
			if (x == 0)
				position.x -= sqrt3 /2.0f * sideLength;
			else
				position.x -= sqrt3 * sideLength;
		}
		if ((x == 0 && !isEven) || (x == xdim-1 && isEven))
			position.z -= 1.5f * sideLength;

		Transform nextHex = Instantiate(transform, position, rotation) as Transform;
		Hexagon next = nextHex.GetComponent<Hexagon>();

		if (isEven) {
			if (x == xdim-1)
				linkTo(2,next);
			else
				linkTo(1,next);
		}
		else {
			if (x == 0)
				linkTo(3,next);
			else
				linkTo(4,next);
		}
		//populate array here
		if (isEven) {
			if (x > 0)
				hexarray[x-1] = buffer;
			else
				hexarray[x] = buffer;
			if (y == 0 && x == xdim-1)
				hexarray[x] = GetComponent<Hexagon>();
		}
		else {
			if (x == xdim-1)
				hexarray[x] = buffer;
			else
				hexarray[x+1] = buffer;
		
		}
		buffer = this;
		
		if (isEven) {
			if (x == xdim-1) {
				next.init(x,y+1);
				return;
			}
			next.init(x+1,y);
			return;
		}
		if (x == 0) {
			next.init(x,y+1);
			return;
		}
		next.init(x-1,y);
		return;
	}
	void Awake () {
		if (triangleMesh == null)
			triangleMesh = GameObject.Find("Triangle");
		Color temp = triangleMesh.renderer.material.color;
		temp.a = 0f;
		triangleMesh.renderer.material.color = temp;
		for (int i = 0; i < 6; ++i) {
			triangles[i] = new Triangle();
			Vector3 position = transform.position;
			//position.x -= (float) (sideLength /sqrt3 * Math.Cos(60));
			//position.z += (float) (sideLength / sqrt3 * Math.Sin(30 * i +15));
			triangles[i].transform = Instantiate(triangleMesh.transform, position, Quaternion.identity) as Transform;
			triangles[i].transform.Rotate(Vector3.up, 60 * (i+1));
			position = triangles[i].transform.position;
			position.z += (float) (Math.Cos (Math.PI * (i+0.5) / 3) / sqrt3 );
			position.x += (float) (Math.Sin (Math.PI * (i+0.5) /3) / sqrt3);
			position.y += 0.05f;
			triangles[i].transform.position = position;
		}
	}
	void Start () {
		if (!begun) {
			begun = true;
			initWrapper();
		}
	}
}
