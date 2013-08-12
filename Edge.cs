using UnityEngine;
using System.Collections;


public class Edge {
	public Edge(Triangle dst, float dist){
		dest = dst;
		distance = dist;
	}
	public Triangle dest;
	public float distance;
}
