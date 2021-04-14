using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk
{
    private Vector2 position; //every 1000 blocks, centered on the coord
    private NoiseSettings noiseSettings;
    private OreType[] oreTypes;
    private float threshold;
    private float[] oreRanges;

    private List<OreVein> localVeins = new List<OreVein>();

    public OreChunk(Vector2 position, NoiseSettings noiseSettings, OreType[] oreTypes, float oreSpawnThreshold)
    {
        this.position = position;
        this.noiseSettings = noiseSettings;
        this.oreTypes = oreTypes;
        //this.threshold = threshold;


        float[,] noise = Noise.GenerateNoiseMap(100, 100, noiseSettings, position * 100); // generates noise map based on worldgrid

        float totalFrequency = 0;
        foreach (OreType ore in oreTypes)
        {
            totalFrequency += ore.frequency;
        }
        foreach (OreType ore in oreTypes)
        {
            ore.relFrequency = ore.frequency / totalFrequency;
        }

        oreRanges = new float[oreTypes.Length];
        float workingSum = 0;
        for (int i = 0; i < oreTypes.Length; i++) 
        {
            workingSum += oreTypes[i].relFrequency;
            oreRanges[i] = workingSum;
            Debug.Log(oreRanges[i]);
        }

        Dictionary<Vector2, float> aboveThreshold = new Dictionary<Vector2, float>();

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (noise[i, j] > threshold)
                {
                    aboveThreshold.Add(new Vector2(position.x * 100 - 50 + i, position.y * 100 - 50 + j), noise[i, j]);
                }
            }
        }
        int loopCount = 0;
        while (aboveThreshold.Count > 0)
        {
            Vector2[] keys = new Vector2[aboveThreshold.Keys.Count];
            aboveThreshold.Keys.CopyTo(keys, 0);
            localVeins.Add(GenerateOreVeinsHelper(keys[0], aboveThreshold, new OreVein(new List<OreSpace>(), null), RandomOreType()));
            loopCount++;
            if (loopCount > 10000)
            {
                Debug.Log("too many loops");
                break;
            }
        }

        //Debug.Log("localveins: " + localVeins.Count);







    }
    
    private OreType RandomOreType()
    {
        float random = Random.Range(0, 1);
        for (int i = 0; i < oreRanges.Length; i++)
        {
            if (random < oreRanges[i])
            {
                return oreTypes[i];
            }
        }
        return oreTypes[oreTypes.Length - 1];
    }



    private OreVein GenerateOreVeinsHelper(Vector2 startAt, Dictionary<Vector2, float> notInVein, OreVein workingOreVein, OreType type)
    {

        Vector2 top = startAt + Vector2.up;
        Vector2 bottom = startAt - Vector2.up;
        Vector2 right = startAt + Vector2.right;
        Vector2 left = startAt - Vector2.right;

        if (workingOreVein.oreSpaces.Count == 0)
        {
            workingOreVein.oreSpaces.Add(new OreSpace(startAt, 0, workingOreVein));
        }

        if (notInVein.ContainsKey(startAt))
        {
            notInVein.Remove(startAt);
        }

        if (notInVein.ContainsKey(top))
        {
            workingOreVein.oreSpaces.Add(new OreSpace(top, (int)(1000 * notInVein[top] * type.richness), workingOreVein));
            notInVein.Remove(top);
            GenerateOreVeinsHelper(top, notInVein, workingOreVein, type);
        }

        if (notInVein.ContainsKey(bottom))
        {
            workingOreVein.oreSpaces.Add(new OreSpace(bottom, (int)(1000 * notInVein[bottom] * type.richness), workingOreVein));
            notInVein.Remove(bottom);
            GenerateOreVeinsHelper(bottom, notInVein, workingOreVein, type);
        }

        if (notInVein.ContainsKey(right))
        {
            workingOreVein.oreSpaces.Add(new OreSpace(right, (int)(1000 * notInVein[right] * type.richness), workingOreVein));
            notInVein.Remove(right);
            GenerateOreVeinsHelper(right, notInVein, workingOreVein, type);
        }

        if (notInVein.ContainsKey(left))
        {
            workingOreVein.oreSpaces.Add(new OreSpace(left, (int)(1000 * notInVein[left] * type.richness), workingOreVein));
            notInVein.Remove(left);
            GenerateOreVeinsHelper(left, notInVein, workingOreVein, type);
        }

        return workingOreVein;
    }


    public Vector2 GetPosition()
    {
        return position;
    }

    public List<OreVein> GetOreVeins()
    {
        return localVeins;
    }

    public OreSpace GetOreSpace(Vector2 gridPosition)
    {
        foreach (OreVein vein in localVeins)
        {
            if (vein.InOreVein(gridPosition))
            {
                return vein.GetOreSpace(gridPosition);
            }
        }
        Debug.Log("no space found");
        return null;
    }
}
