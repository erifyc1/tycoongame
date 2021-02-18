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
    private BuildUI buildUI;

    void Start()
    {
        buildUI = GameObject.FindGameObjectWithTag("buildmanager").GetComponent<BuildUI>();
    }

    void OnGUI()
    {
        if (!placed)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x, Camera.main.pixelHeight - Event.current.mousePosition.y, Camera.main.transform.position.y));
            Debug.Log(point);

            closestX = point.x % 10 < 5 ? Mathf.FloorToInt(point.x / 10) * 10 : Mathf.CeilToInt(point.x / 10) * 10;
            closestZ = point.z % 10 < 5 ? Mathf.FloorToInt(point.z / 10) * 10 : Mathf.CeilToInt(point.z / 10) * 10;
            transform.position = new Vector3(closestX, 0, closestZ);
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !buildUI.GetOccupiedTiles().Contains(new Vector2(transform.position.x, transform.position.z)))
        {
            placed = true;
            Cursor.visible = true;
            buildUI.placingObject = false;
			buildUI.GetOccupiedTiles().Add(new Vector2(transform.position.x, transform.position.z));
        }
    }
}