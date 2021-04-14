using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkOres
{
	OreType[] oreTypes;
	float oreSpawnThreshold;
	float[] oreRanges;
	private List<OreVein> localVeins = new List<OreVein>();

	public ChunkOres(OreType[] ots, float threshold)
    {
		oreTypes = ots;
		oreSpawnThreshold = threshold;
    }

    public void GenerateOres(Vector2 coord, float meshWorldSize, float meshScale, NoiseSettings oreNoiseSettings)
    {
        Vector2 position = coord * meshWorldSize / 10; // center of chunk world grid
        int gridSideLength = (int)meshWorldSize / 10; // 850 meshworldsize testing

        float[,] noise = Noise.GenerateNoiseMap(gridSideLength, gridSideLength, oreNoiseSettings, position); // generates noise map based on worldgrid

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
        }

        Dictionary<Vector2, float> aboveThreshold = new Dictionary<Vector2, float>();

        for (int i = 0; i < gridSideLength; i++)
        {
            for (int j = 0; j < gridSideLength; j++)
            {
                if (noise[i, j] > oreSpawnThreshold)
                {
                    aboveThreshold.Add(new Vector2(position.x - (int)(0.5 * gridSideLength) + i, position.y - (int)(0.5 * gridSideLength) + j), noise[i, j]);

                    //Debug.Log(new Vector2(position.x - (int)(0.5 * gridSideLength) + i, position.y - (int)(0.5 * gridSideLength) + j));
                    //Debug.Log(gridSideLength);
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

    }

	public OreVein GenerateOreVeinsHelper(Vector2 startAt, Dictionary<Vector2, float> notInVein, OreVein workingOreVein, OreType type)
	{
		Vector2 top = startAt + Vector2.up;
		Vector2 bottom = startAt - Vector2.up;
		Vector2 right = startAt + Vector2.right;
		Vector2 left = startAt - Vector2.right;

		if (workingOreVein.oreSpaces.Count == 0)
		{
			workingOreVein.oreSpaces.Add(new OreSpace(startAt, (int)(1000 * notInVein[startAt] * type.richness), workingOreVein));
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


	public void ShowOres(Vector3[] verts)
    {
        List<Vector3> chunkVerts = new List<Vector3>();
        foreach (Vector3 point in verts)
        {
            if (point.x % 10 == 0 && point.y % 10 == 0 && point.z % 10 == 0)
            {
                chunkVerts.Add(point);
            }
        }

        foreach (OreVein vein in localVeins)
        {
            GameObject oreMeshObj = new GameObject();
            List<Vector2> vertices2D = new List<Vector2>();
            for (int i = 0; i < vein.oreSpaces.Count; i++)
            {
                Vector2 center = 10 * vein.oreSpaces[i].location;
                vertices2D.Add(new Vector2(center.x - 5, center.y - 5));
                vertices2D.Add(new Vector2(center.x + 5, center.y - 5));
                vertices2D.Add(new Vector2(center.x + 5, center.y + 5));
            }

            Vector2[] v2D = new Vector2[vertices2D.Count];
            vertices2D.CopyTo(v2D);
            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(v2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                //Debug.Log(vertices2D[i]);
                //Vector3 point = chunkVerts.Find((vert) => vert.x == vertices2D[i].x * 10 && vert.z == vertices2D[i].y * 10);
                if (true)//point != null)
                {
                    //Debug.Log(point); 
                    vertices[i] = new Vector3(v2D[i].x, 100, v2D[i].y);
                }
                else
                {
                    Debug.Log("existn't");
                    vertices[i] = Vector3.right;
                }
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            // Set up game object with mesh;
            oreMeshObj.AddComponent(typeof(MeshRenderer));
            MeshFilter filter = oreMeshObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = msh;
            
        }
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
