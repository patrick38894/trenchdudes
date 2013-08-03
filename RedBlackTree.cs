using UnityEngine;
using System.Collections;


public class RedBlackTree {
	
	protected const bool RED = true;
	protected const bool BLACK = false;
	
	protected Node root;
	
	protected class Node {
		public Triangle data;
		public Node left, right, parent;
		public bool color;
		public float key = 0f;
		
		public Node(Triangle newData) {
			data = newData;
			key = newData.f;
			left = right = parent = null;
			color = RED; //by default i guess this makes sense
		}
	}
	
	protected Node insert(Node head, Triangle newData) {
		if (head.key > newData.f) {
			if (head.left != null)
				head.left = insert(head.left, newData);
			else {
				head.left = new Node(newData);
				head.left.parent = head;
			}
			insertCase1(head.left);
		}
		else {
			if (head.right != null)
				head.right = insert(head.right, newData);
			else {
				head.right = new Node(newData);
				head.right.parent = head;
			}
			insertCase1(head.right);
		}
		return head;
	}
		
	public void insert(Triangle newData) {
		if (root != null)
			root = insert(root, newData);
		else
			root = new Node(newData);
	}
		
	protected void insertCase1(Node n) {
	//this function is called on any node immediately after insertion
 
		if (n.parent == null)
			n.color = BLACK;
		else
			insertCase2(n);
	}
	
	protected Node uncle(Node n) {
		Node g = grandparent(n);
		if (g == null)
			return null;
		if (n.parent == g.left)
			return g.right;
		else
			return g.left;
	}
	
	protected Node grandparent(Node n) {
		if ((n != null) && (n.parent != null))
			return n.parent.parent;
		else
			return null;
	}

 
	protected void insertCase2(Node n) {
		if (n.parent.color == BLACK)
			return;
		else
			insertCase3(n);
	}
 
	protected void insertCase3(Node n) {
		Node u = uncle(n);
		if (u != null && u.color == RED) {
			n.parent.color = BLACK;
			u.color = BLACK;
			Node g = grandparent(n);
			g.color = RED;
			insertCase1(g);
		}
		else
			insertCase4(n);
	}
 
	protected void insertCase4(Node n) {
		Node g = grandparent(n);
		if ((n == n.parent.right) && (n.parent == g.left)) {
			rotateLeft(n.parent);
			n = n.left;
		}
		else if ((n == n.parent.left) && (n.parent == g.right)) {
			rotateRight(n.parent);
			n = n.right;
		}
		else
			insertCase5(n);
	}
 
	protected void insertCase5(Node n) {
		Node g = grandparent(n);
		n.parent.color = BLACK;
		g.color = RED;
		if (n == n.parent.left)
			rotateRight(g);
		else
			rotateLeft(g);
	}
 
	protected void rotateRight(Node n) {
		// n is parent.left
		Node temp = n.left;
		temp.parent = n.parent;
		temp.parent.left = temp;
		n.left = temp.right;
		if (n.left != null)
			n.left.parent = n;
		temp.right = n;
		n.parent = temp;
	}
			   
 
	protected void rotateLeft(Node n) {
		// n is parent.right
		Node temp = n.right;
		temp.parent = n.parent;
		temp.parent.right = temp;
		n.right = temp.left;
		if (n.right != null)
			n.right.parent = n;
		temp.left = n;
		n.parent = temp;
	}
	
	protected Triangle pop(Node head) {	
		if (head == null)
			return null;
		if (head.left == null) {
			//we have reached the smallest item
			Triangle temp = head.data;
			if (head.right != null) {
					//head must be black && head.right must have no children
					head.parent.left = head.right;
					head.right.color = BLACK;
			}
			else {
				if (head.color == RED)
					head.parent.left = null;
				else {
					Node p = head.parent;
					Node s = p.right;
					if (s.color == RED) {
						//there must be exactly 2 black children
						//with no more than 2 children each
						p.left = null;
						rotationOn(p);
						p.color = RED; 
						s.color = BLACK;
					}
					else {
						if (s.left != null) {
							Node sl = s.left;
							sl.parent = grandparent(head);
							s.left = head.parent;
							head.parent.parent = sl;
							head.parent.left = null;
							sl.right = s;
							s.parent = sl;
							head.parent.color = BLACK;
						}
						else if (s.right != null) {
							g = grandparent(head);
							g.left = s;
							s.parent = g;
							s.left = p;
							p.parent = s;
							p.left = null;
							p.color = BLACK;
							s.color = RED;
							s.right.color = BLACK;
						}
						else {
							p.color = BLACK;
							s.color = RED;
							p.left = null;
						}
					}
				}
							
							
					}
				head.parent.left = null;
				
				
				
			return temp;
		}
		return pop(head.left);
	}
	
	public Triangle pop() {
		return pop(root);
	}
}
