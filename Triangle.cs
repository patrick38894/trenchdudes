using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Triangle {
	class Edge {
		public Edge(Triangle dst, float dist){
			dest = dst;
			distance = dist;
		}
		public Triangle dest;
		public float distance;
	}


	public static void doubleLink(Triangle src, Triangle dst, float dist){
		dst.link(src, dist);
		src.link(dst, dist);
	}

	public void link(Triangle src, float dist){
		adjacent.Add(new Edge(src, dist));
	}
	public Transform transform;
	List<Edge> adjacent = new List<Edge> ();
}


