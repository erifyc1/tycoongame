using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
	[SerializeField]
	GameObject cubePre;
	[SerializeField]
	private List<Vector2> occupiedTiles = new List<Vector2>();
	public bool placingObject = false;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Cube()
	{
		if (!placingObject)
		{
			placingObject = true;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Vector3 rayPoint = ray.GetPoint(hit.distance);
				GameObject cube = Instantiate(cubePre, rayPoint, Quaternion.Euler(0, 180, 0));
			}
		}
	}

	public List<Vector2> GetOccupiedTiles()
	{
		return occupiedTiles;
	}
}
