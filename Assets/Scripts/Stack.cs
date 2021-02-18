using System;
using UnityEngine;

public class Stack {
	public float x { get; }
	public float y { get; }
	public int height { get; }
	public GameObject obj { get; }
	
	public Stack(float x, float y, int height, GameObject obj) {
		this.x = x;
		this.y = y;
		this.height = height;
		this.obj = obj;
	}
}