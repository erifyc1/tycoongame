using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(0, 1, -10);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W) == true)
		{
			transform.position += new Vector3(1, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.S) == true)
		{
			transform.position += new Vector3(-1, 0, 0);
		}
	}
}
