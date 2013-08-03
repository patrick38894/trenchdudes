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
	protected static RedBlackTree perimeter = new RedBlackTree();
	
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
				perimeter.insert(current.dest);
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
	
	public Triangle AStar () {
		Triangle temp = setgf();
		if (temp != null)
			return temp;
		adjacent.Sort(compareFValues);
		foreach (Edge current in adjacent)
			current.dest.checkShortcuts();
		
		
		
	}
	
	
	public Triangle setgf() {
		//visitState = 2; //g = f = 0
		if (heuristic == 0)
			return this;
		foreach (Edge current in adjacent) {
			if (current.dest.visitState == 0) {
				if (current.dest.terrain & TerrainType.passable != TerrainType.passable) {
					current.dest.visitState = 4;
					continue;
				}
				current.dest.g = terrainCost(terrain, current.dest.terrain) * current.distance + g;
				current.dest.f = current.dest.g + current.dest.heuristic;
				current.dest.parent = this;
				current.dest.visitState = 1;
			}
		}
		
		adjacent.Sort(compareFValues);
		foreach (Edge current in adjacent) {
			current.dest.checkShortcuts();
			Triangle temp = current.dest.setgf();
			if (temp != null)
					return temp;
		}
		return null;
	}
	
	public static int compareFValues(Edge a, Edge b) {
		if (a == null || a.dest == null) {
			if (b == null || b.dest == null)
				return 0;
			else
				return -1;
		}
		else {
			if (b == null || b.dest == null)
				return 1;
			else {
				if (a.dest.f > b.dest.f)
					return 1;
				else
					return -1;
			}
		}
	}
	
	public void reset() {
		visitState = 8;
		foreach (Edge current in adjacent) {
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


