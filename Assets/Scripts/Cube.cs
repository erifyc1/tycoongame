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
}