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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject cube = Instantiate(cubePre, ray.GetPoint(hit.distance), Quaternion.Euler(0, 180, 0));
            }
        }

    }
}
