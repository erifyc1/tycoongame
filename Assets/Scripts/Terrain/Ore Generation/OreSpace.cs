using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpace
{
    public Vector2 location;
    public int oreCount;
    public OreVein oreVein;
    public OreSpace(Vector2 location, int oreCount, OreVein oreVein)
    {
        this.location = location;
        this.oreCount = oreCount;
        this.oreVein = oreVein;
    }
}
