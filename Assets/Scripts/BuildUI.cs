using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField]
    GameObject cubePre;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cube()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject cube = Instantiate(cubePre, ray.GetPoint(hit.distance), transform.rotation);
        }

    }
}