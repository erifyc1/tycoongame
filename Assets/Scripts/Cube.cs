using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

	private bool placed = false;
	[SerializeField]
	Vector3 mousePos;
	private float closestX;
	private float closestZ;

	void OnGUI()
	{
		if (!placed)
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x, Camera.main.pixelHeight - Event.current.mousePosition.y, Camera.main.transform.position.y));
			Debug.Log(point);

			if ((point.x % 10 < 2 || point.x % 10 > 8) && (point.y % 10 < 2 || point.y % 10 > 8))
			{
				closestX = point.x % 10 < 2 ? Mathf.FloorToInt(point.x / 10) * 10 : point.x % 10 > 8 ? Mathf.CeilToInt(point.x / 10) * 10 : point.x;
				closestZ = point.z % 10 < 2 ? Mathf.FloorToInt(point.z / 10) * 10 : point.z % 10 > 8 ? Mathf.CeilToInt(point.z / 10) * 10 : point.z;
				transform.position = new Vector3(closestX, 0, closestZ);
			}
			else
			{
				transform.position = point;
			}

		}
	}
}