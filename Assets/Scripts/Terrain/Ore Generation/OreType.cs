using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OreType
{
    public string name;
    public ResourceType resourceType;
    [Range(0.1f, 5f)]
    public float frequency;
    [HideInInspector]
    public float relFrequency;
    [Range(0.1f, 5f)]
    public float richness;
}
