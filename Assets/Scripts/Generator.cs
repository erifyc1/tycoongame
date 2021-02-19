using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IActivatable
{
	[SerializeField]
	GameObject resourceCube;
	[SerializeField]
	float spawnTime = 5f;
	private float timer = 0;
	void Start()
	{

	}
	public void Activate()
	{
		Debug.Log("yay");
	}

	public void Deactivate()
	{
		Debug.Log("unyay");
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > spawnTime)
		{
			timer = 0;
			if (resourceCube != null)
			{
				Instantiate(resourceCube, new Vector3(transform.position.x, transform.position.y, transform.position.z) + 2 * Vector3.up, new Quaternion(0, 0, 0, 0));
			}
		}
	}
}
