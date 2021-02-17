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
            transform.position = new Vector3(mouseX, 0, mouseY);
        }
    }
}