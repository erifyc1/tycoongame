using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField]
    GameObject cubePre;
    private int[][] occupiedTiles = { };
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
            GameObject cube = Instantiate(cubePre, new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
        }

    }
}
