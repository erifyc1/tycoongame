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
    void Update()
    {
        if (!placed)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                mousePos = new Vector3(ray.GetPoint(hit.distance).x, 0, ray.GetPoint(hit.distance).z);
                closestX = mousePos.x % 10 < 5 ? Mathf.FloorToInt(mousePos.x / 10) * 10 : Mathf.CeilToInt(mousePos.x / 10) * 10;
                closestZ = mousePos.z % 10 < 5 ? Mathf.FloorToInt(mousePos.z / 10) * 10 : Mathf.CeilToInt(mousePos.z / 10) * 10;
                transform.position = new Vector3(closestX, 0, closestZ);
            }
        }
    }
}