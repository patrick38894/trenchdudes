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
	public List<Edge> adjacent = new List<Edge> ();
	protected static AATree<float,Triangle> perimeter = new AATree<float,Triangle>();
	protected static Queue<Triangle> hqueue = new Queue<Triangle>();
	
	class TerrainType {
		public static int passable = 1;
	}
	


	public static void doubleLink(Triangle src, Triangle dst, float dist){
		dst.link(src, dist);
		src.link(dst, dist);
	}

	public void link(Triangle src, float dist){
		adjacent.Add(new Edge(src, dist));
	}
	
	
	/*public void setHeuristics(int num) {
		f = g = 0;
		heuristic = num;
		visitState = 1;
		foreach (Edge current in adjacent) {
			if ((current.dest.visitState != 1) && (current.distance == Hexagon.diamond))
				perimeter.Add(heuristic, current.dest);
				current.dest.visitState = 1;
		}
		
		Triangle t = perimeter.pop ();
		while (t != null) {
			t.setHeuristics(num +1);
			t = perimeter.pop ();
		}
	}*/
	
	public void setColor(Color c) {
		transform.renderer.material.color = c;
	}
	public void setHeuristics() {
		heuristic = 0;
		visitState = 1;
		hqueue.Clear();
		foreach (Edge current in adjacent) {
			//current.dest.setColor(Color.red);
			current.dest.visitState = 1;
			current.dest.heuristic = heuristic + 1;
			hqueue.Enqueue(current.dest);
		}
		Triangle t = hqueue.Dequeue();
		while (true) {
			foreach (Edge next in t.adjacent) {
				if (next.dest.visitState != 1) {
					//next.dest.setColor(Color.red);
					next.dest.visitState = 1;
					next.dest.heuristic = t.heuristic + 1;
					hqueue.Enqueue(next.dest);
				}
			}
			if (hqueue.Count > 0)
				t = hqueue.Dequeue();
			else
				break;
		}
	}
	
	public static void orderFlip(Triangle start, ref Stack<Triangle> stack) {
		if (start == null || start.parent == null)
			return; //new Stack<Triangle>();
		//Stack <Triangle> stack = new Stack<Triangle>();
		stack.Push (start);
		Triangle temp = start.parent;
		while (temp != null) {
			stack.Push (temp);
			temp = temp.parent;
			Debug.Log ("one iteration");
		}
		
		//return stack;
		/*temp = stack.Pop ();
		Triangle retval = temp;
		while (stack.Count > 0) {
			temp.parent = stack.Pop ();
			temp = temp.parent;
		}
		temp.parent = null;
		return retval;*/
	}
	
	public static int terrainCost(int start, int end) {
		return 1; //for now	
	}
	
	public Triangle path(Triangle dest) {
		reset();
		Debug.Log ("reset1 complete");
		reset2 ();
		Debug.Log ("reset 2 complete");
		dest.setHeuristics();
		Debug.Log ("heuristics have been set");
		reset2 ();
		return AStar();
	}
	
	/*protected Triangle AStar () {
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
	}*/

	
	protected Triangle AStar () {
	//to be called on the starting position
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
				perimeter.Add(current.dest.f, current.dest);
			}
		}
		
		Triangle temp = perimeter.pop ();
		
		while (temp != null) {
			temp.checkShortcuts();
			foreach (Edge next in temp.adjacent) {
				//temp.setColor(Color.cyan);
				if (next.dest.visitState == 0) {
					if (next.dest.heuristic == 0) {
						next.dest.parent = temp;
						return next.dest;
					}
					if ((next.dest.terrain & TerrainType.passable) != TerrainType.passable) {
						next.dest.visitState = 4;
						continue;
					}
					next.dest.g = terrainCost(temp.terrain, next.dest.terrain) * next.distance + temp.g;
					next.dest.f = next.dest.g + next.dest.heuristic;
					next.dest.parent = temp;
					next.dest.visitState = 1;

					perimeter.Add(next.dest.f, next.dest);
				}
			}
			temp = perimeter.pop ();
		}
		Debug.Log ("pathing error");
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
	}
			
	public void reset2() {
		visitState = 0;
		foreach (Edge current in adjacent) {
			//if (current == null || current.dest == null)
			//	Debug.Log ("null error");
			if (current.dest.visitState != 0)
				current.dest.reset2();
		}
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


