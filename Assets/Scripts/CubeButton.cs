using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour
{

    private GameObject BM;
    void Start()
    {
        BM = GameObject.FindGameObjectWithTag("buildmanager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCube()
    {
        BM.GetComponent<BuildUI>().Cube();
    }
}
