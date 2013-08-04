using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle {
		
	public int terrain;
	public int heuristic;
	public Triangle parent;
	public float f;
	public float g;
	public short visitState; // 0: unvisited, 1: visited, 2: on closed list, 4: impassable, 8: other
	public Transform transform;
	List<Edge> adjacent = new List<Edge> ();
	protected static AATree<float,Triangle> perimeter = new AATree<float,Triangle>();
	
	class TerrainType {
		public static int passable = 1;
	}
	
	public class Edge {
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
	
	
	public void setHeuristics(int num) {
		g = 0;
		f = heuristic = num;
		visitState = 1;
		foreach (Edge current in adjacent) {
			if ((current.dest.visitState != 1) && (current.distance == Hexagon.diamond))
				perimeter.Add(current.dest.f, current.dest);
		}
		
		Triangle t;
		do {
			t = perimeter.pop();
			if (t != null)
				t.setHeuristics(num+1);
		} while (t != null);
		
		visitState = 0;
		f = 0;
	}
	
	public static int terrainCost(int start, int end) {
		return 1; //for now	
	}
	
	public Triangle path(Triangle dest) {
		reset();
		dest.setHeuristics(0);
		return AStar();
	}
	
	protected Triangle AStar () {
		if (heuristic == 0)
			return this;
		foreach (Edge current in adjacent) {
			if (current.dest.visitState == 0) {
				if ((current.dest.terrain & TerrainType.passable) != TerrainType.passable) {
					current.dest.visitState = 4;
					continue;
				}
				current.dest.g = terrainCost(terrain, current.dest.terrain) * current.distance + g;
				current.dest.f = current.dest.g + current.dest.heuristic;
				current.dest.parent = this;
				current.dest.visitState = 1;
			}
		}
		
		
		foreach (Edge current in adjacent) {
			perimeter.Add(current.dest.f, current.dest);
		}
		
		Triangle temp, temp2;
		
		while ((temp = perimeter.pop()) != null) {
				temp.checkShortcuts();
				if ((temp2 = temp.AStar()) != null)
					return temp2;
		}
		return null;
	}

	
	public void reset() {
		visitState = 8;
		foreach (Edge current in adjacent) {
			if (current == null || current.dest == null)
				Debug.Log ("null error");
			if (current.dest.visitState != 8)
				current.dest.reset();
		}
		visitState = 0;
	}
			
			
	public void checkShortcuts() {
		visitState = 2;
		foreach (Edge current in adjacent) {
			if (current.dest.visitState == 1)
				if (current.dest.g > g + terrainCost(terrain, current.dest.terrain))
					current.dest.parent = this;
	
		}
	}
}


