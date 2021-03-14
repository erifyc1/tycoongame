using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{

    private Vector2 position;
    private float addInfluence;
    private float multInfluence;
    private int influenceRange;

    GameObject g;
    public Biome(Vector2 pos, float addInfluence, float multInfluence, int range, GameObject sphere) // influence types: 0 multiplicative, 1 additive/subtractive
    {
        position = pos;
        this.addInfluence = addInfluence;
        this.multInfluence = multInfluence;
        influenceRange = range;
        g = GameObject.Instantiate(sphere);
        g.transform.position = new Vector3(position.x, 25, position.y);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public float GetDistanceToBiome(Vector3 point)
    {
        return Vector3.Distance(new Vector3(position.x, 0, position.y), point);
    }

    public float GetInfluenceAt(Vector3 point, InfluenceType type) // point.y should = 0
    {
        float influence = 0;
        float distance = GetDistanceToBiome(point);


        if (type == InfluenceType.Additive)
        {
            influence = addInfluence;
            if (distance > influenceRange) return 0;
        }
        else if (type == InfluenceType.Multiplicative)
        {
            influence = multInfluence;
            if (distance > influenceRange) return 0;
        }
        else return 0;



        float scale = influenceRange / 69.6607f;
        return influence * (2 - Mathf.Pow(1.01f, distance / scale));
    }

}
