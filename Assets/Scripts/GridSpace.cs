using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    private Vector3 position; // in absolute coordinates
    private GameObject obj;
    private int occupyType; // 0 => object, 1 => ground


    public GridSpace(Vector3 pos, GameObject o)
    {
        position = pos;
        obj = o;
        occupyType = 0;
    }

    public void DestroyGameObject() // use deleteSpace from worldgrid
    {
        if (obj != null)
        {
            GameObject.Destroy(obj);
        }
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public GameObject GetObject()
    {
        return obj;
    }

    public int GetOccupyType()
    {
        return occupyType;
    }

}
