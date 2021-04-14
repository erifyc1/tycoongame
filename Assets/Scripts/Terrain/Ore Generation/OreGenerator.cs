using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGenerator : MonoBehaviour
{
    public OreType[] ores;

    public NoiseSettings oreNoise;

    public GameObject test;

    private List<OreChunk> oreChunks = new List<OreChunk>();

    public Transform viewer;

    private Vector2 viewerOreChunkPosition;

/*    private void Update()
    {
        viewerOreChunkPosition = new Vector2((int)(viewer.position.x / 1000), (int)(viewer.position.z / 1000));
        if (!oreChunks.Exists((chunk) => chunk.GetPosition() == viewerOreChunkPosition))
        {
            OreChunk newOreChunk = new OreChunk(viewerOreChunkPosition, oreNoise, ores, 0.8f);
            oreChunks.Add(newOreChunk);
            ShowOreVeins(newOreChunk);
        }

        if (!oreChunks.Exists((chunk) => chunk.GetPosition() == viewerOreChunkPosition + Vector2.up))
        {
            OreChunk newOreChunk = new OreChunk(viewerOreChunkPosition + Vector2.up, oreNoise, ores, 0.8f);
            oreChunks.Add(newOreChunk);
            ShowOreVeins(newOreChunk);
        }

        if (!oreChunks.Exists((chunk) => chunk.GetPosition() == viewerOreChunkPosition - Vector2.up))
        {
            OreChunk newOreChunk = new OreChunk(viewerOreChunkPosition - Vector2.up, oreNoise, ores, 0.8f);
            oreChunks.Add(newOreChunk);
            ShowOreVeins(newOreChunk);
        }

        if (!oreChunks.Exists((chunk) => chunk.GetPosition() == viewerOreChunkPosition + Vector2.right))
        {
            OreChunk newOreChunk = new OreChunk(viewerOreChunkPosition + Vector2.right, oreNoise, ores, 0.8f);
            oreChunks.Add(newOreChunk);
            ShowOreVeins(newOreChunk);
        }

        if (!oreChunks.Exists((chunk) => chunk.GetPosition() == viewerOreChunkPosition - Vector2.right))
        {
            OreChunk newOreChunk = new OreChunk(viewerOreChunkPosition - Vector2.right, oreNoise, ores, 0.8f);
            oreChunks.Add(newOreChunk);
            ShowOreVeins(newOreChunk);
        }
    }*/

    private void ShowOreVeins(OreChunk chunk)
    {
        foreach (OreVein vein in chunk.GetOreVeins())
        {
            foreach (OreSpace space in vein.oreSpaces)
            {
                Ray ray = new Ray(new Vector3(space.location.x * 10, 1000, space.location.y * 10), Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f, LayerMask.GetMask("ground"), QueryTriggerInteraction.Ignore))
                {
                    GameObject g = Instantiate(test);
                    g.transform.position = ray.GetPoint(hit.distance) + 10 * Vector3.up;
                }
            }
        }
        
    }

    public OreChunk OreChunkFromPosition(Vector2 position)
    {
        Vector2 chunkPos = new Vector2((int)(position.x / 1000), (int)(position.y / 1000));
        OreChunk chunk = oreChunks.Find((chunk) => chunk.GetPosition() == chunkPos);
        if (chunk != null)
        {
            return chunk;
        }
        else
        {
            Debug.Log("specified chunk does not exist");
            return null;
        }
    }

    public OreSpace OreSpaceFromPosition(Vector2 gridPosition)
    {
        OreChunk chunk = OreChunkFromPosition(gridPosition);
        if (chunk != null)
        {
            return chunk.GetOreSpace(gridPosition);
        }
        else
        {
            return null;
        }
    }

    private void GenerateVeinMeshes(OreChunk chunk)
    {
        foreach (OreVein vein in chunk.GetOreVeins())
        {
            Vector2[] vertices2D = new Vector2[vein.oreSpaces.Count];
            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);


                // Create the mesh
                Mesh msh = new Mesh();
                msh.vertices = vertices;
                msh.triangles = indices;
                msh.RecalculateNormals();
                msh.RecalculateBounds();

                // Set up game object with mesh;
                gameObject.AddComponent(typeof(MeshRenderer));
                MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
                filter.mesh = msh;
            }
        }
    }


}
