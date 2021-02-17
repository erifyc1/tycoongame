using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    bool placed = false;
    void Update()
    {
        float mouseX = Input.mousePosition[0];
        float mouseY = Input.mousePosition[1];

        if (!placed)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                transform.position = new Vector3(ray.GetPoint(hit.distance).x, 0, ray.GetPoint(hit.distance).z);
            }
        }
    }
}