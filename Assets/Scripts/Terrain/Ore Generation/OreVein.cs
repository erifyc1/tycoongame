using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreVein
{
    public OreType oreType;
    public List<OreSpace> oreSpaces;
    public OreVein(List<OreSpace> oreSpaces, OreType oreType)
    {
        this.oreSpaces = oreSpaces;
        this.oreType = oreType;
    }

    public bool InOreVein(Vector2 point)
    {
        return oreSpaces.Exists((tile) => tile.location == point);
    }

    public OreSpace GetOreSpace(Vector2 point)
    {
        return oreSpaces.Find((tile) => tile.location == point);
    }
}
