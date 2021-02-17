using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(0, 1, -10);
		transform.rotation = new Quaternion(0, 0, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			transform.Rotate(-5.0f, 0.0f, 0.0f);
			Debug.Log("W Pressed");
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			transform.Rotate(5.0f, 0.0f, 0.0f);
			Debug.Log("S Pressed");
		}
	}
}
