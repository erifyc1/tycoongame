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

            mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 0, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
            Debug.Log(mousePos);


            if ((mousePos.x % 10 < 2 || mousePos.x % 10 > 8) && (mousePos.y % 10 < 2 || mousePos.y % 10 > 8))
            {
                closestX = mousePos.x % 10 < 2 ? Mathf.FloorToInt(mousePos.x / 10) * 10 : mousePos.x % 10 > 8 ? Mathf.CeilToInt(mousePos.x / 10) * 10 : mousePos.x;
                closestZ = mousePos.z % 10 < 2 ? Mathf.FloorToInt(mousePos.z / 10) * 10 : mousePos.z % 10 > 8 ? Mathf.CeilToInt(mousePos.z / 10) * 10 : mousePos.z;
                transform.position = new Vector3(closestX, 0, closestZ);
            }
            else
            {
                transform.position = mousePos;
            }

        }
    }
}