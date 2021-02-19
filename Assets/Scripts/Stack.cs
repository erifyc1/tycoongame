using System;
using System.Collections.Generic;
using UnityEngine;

public class Stack {
	public float x { get; }
	public float y { get; }
	public int height { get; private set; }
	public bool capped { get; private set; }
	public List<GameObject> objs { get; } = new List<GameObject>();
	
	public Stack(float x, float y, int height, GameObject obj) {
		this.x = x;
		this.y = y;
		this.height = height;
		capped = obj.GetComponents<MonoBehaviour>().Length != 0 && !(obj.GetComponents<MonoBehaviour>()[0] is IStackable);
		objs.Add(obj);
	}

	public void StackObject(GameObject obj) {
		if (!capped) {
			objs.Add(obj);
			height += 10;
			capped = obj.GetComponents<MonoBehaviour>().Length != 0 && !(obj.GetComponents<MonoBehaviour>()[0] is IStackable);
		} else {
			Debug.LogWarning("Tried to stack on a capped stack");
		}
	}

	public GameObject PopObject() {
		GameObject removedElement = objs[objs.Count - 1];
		objs.RemoveAt(objs.Count - 1);
		return removedElement;
	}
}