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
	GameObject currPlacingObject;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (placingObject) {
			Vector3 objPos = currPlacingObject.transform.position;
			
			if (Input.GetMouseButton(0) && !occupiedTiles.Contains(new Vector2(objPos.x, objPos.z)))
			{
				Stack stack = new Stack(objPos.x, objPos.z, Mathf.FloorToInt(objPos.y/10), currPlacingObject);
				occupiedTiles.Add(new Vector2(objPos.x, objPos.z));
				Cursor.visible = true;
				placingObject = false;
				currPlacingObject = null;
			}
		}
    }

    public void Cube()
    {
        if (!placingObject)
        {
            GameObject cube = Instantiate(cubePre, new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
            placingObject = true;
			currPlacingObject = cube;
        }
    }

    void OnGUI()
    {
        if (placingObject)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x, Camera.main.pixelHeight - Event.current.mousePosition.y, Camera.main.transform.position.y));

            float closestX = point.x % 10 < 5 ? Mathf.FloorToInt(point.x / 10) * 10 : Mathf.CeilToInt(point.x / 10) * 10;
            float closestZ = point.z % 10 < 5 ? Mathf.FloorToInt(point.z / 10) * 10 : Mathf.CeilToInt(point.z / 10) * 10;
            currPlacingObject.transform.position = new Vector3(closestX, 0, closestZ);
            Cursor.visible = false;
        }
    }
}