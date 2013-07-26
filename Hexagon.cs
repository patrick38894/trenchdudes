using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
class Hexagon : MonoBehaviour {
	public Triangle [] triangles = new Triangle[6];
	public Hexagon [] hexagons = new Hexagon[6];
	public static float diamond = 1f / 2f * (float) Math.Sqrt(3);
	public static float hourglass = 2f / (float) Math.Sqrt(3);
	public static float sideLength = 1.0f;
	public static float sqrt3 = (float) Math.Sqrt(3);
	protected static bool begun = false;
	
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
	}
	
	protected static Hexagon [] hexarray;
	public static int xdim = 8;
	public static int ydim = 8;
	
	public void initWrapper() {
		hexarray = new Hexagon[xdim];
		hexarray[0] = GetComponent<Hexagon>();
		init(0, 0);
	}

	public void init (int x, int y) {
		Vector3 position = transform.position;
		bool isEven = (y%2 ==0);
		
		if (isEven &&  y!=0) {//link upwards leaning to the right
			linkTo(0,hexarray[x]);
			if (x > 0)
				linkTo(5,hexarray[x-1]);
		}
		else if (! isEven) {
			linkTo(5,hexarray[x]);
			if (x < xdim -1) {
				linkTo(0,hexarray[x+1]);
		}
		
		if (y == ydim-1)
			if ((isEven && x == xdim-1) || (!isEven && x == 0))
				return;
		if (isEven)
			position.x += sqrt3 * sideLength; 
		else
			position.x -= sqrt3 * sideLength; 
		if (x == 0 || x == xdim-1)
			position.x -= sideLength;

		GameObject nextHex = Instantiate(this, position, Quaternion.identity) as GameObject;
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
		hexarray[x] = hexagons[0];
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
	}
	public void Start () {
		if (!begun) {
			begun = true;
			initWrapper();
		}
	}
}
