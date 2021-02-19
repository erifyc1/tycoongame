using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectPrefabs;

    private int[][] occupiedTiles = { };
    public bool placingObject = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildObject(int id)
    {
        if (!placingObject)
        {
            if (objectPrefabs[id] != null) {
                placingObject = true;
                GameObject object = Instantiate(objectPrefabs[id], Vector3.zero);
            }
            else
            {
                Debug.Log("invalid object id");
            }

        }

    }
}
