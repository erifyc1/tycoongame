using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour
{

	private GameObject BM;
	void Start()
	{
		BM = GameObject.FindGameObjectWithTag("buildmanager");
		// transform.position = new Vector3(-Screen.width / 2 + 80, -Screen.height / 2 + 15, 0);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SpawnCube()
	{
		BM.GetComponent<BuildUI>().BuildObject(0);
	}
}
