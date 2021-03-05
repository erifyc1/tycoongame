using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Unity.Jobs;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] bool enableChunkLoading;
    [SerializeField] Material mat;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] vertexColors;
    public Gradient gradient;
    private float maxTerrainHeight;
    private float minTerrainHeight;
    private float[] biomeFactor;

    private List<Chunk> chunks;
    private Vector3 currOccupiedChunk;
    private CameraScript camScript;

    [SerializeField] float chunkUpdateTime = 3f;
	private float timer = 0f;


    private int xSize = 100; // per chunk
    private int zSize = 100; // per chunk
    private int vertFrequency = 5;
	
    void Start()
    {
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        currOccupiedChunk = Vector3.zero;
        chunks = new List<Chunk>();
        if (vertFrequency < 1) vertFrequency = 1;
        CalcTriangles();
        //CalcRandomHeightVariation();
        //StartCoroutine(GenerateChunk(0, 0));
        StartCoroutine(LoadRadius(currOccupiedChunk, 3));
        
    }

    void CalcRandomHeightVariation()
    {
        float xOffset = UnityEngine.Random.Range(0f, 1f)*5000;
        float zOffset = UnityEngine.Random.Range(0f, 1f)*5000;
        // biomeFactor = new float[201*201];
        // for (int z = 0, i = 0; z <= 200; z++)
        // {
        //     for (int x = 0; x <= 200; x++)
        //     {
        //         float y = Mathf.PerlinNoise(xOffset - (0.005f*(x)),  zOffset - (0.005f*(z)));
        //         biomeFactor[i] = y;

        //         i++;
        //     }
        // }
    }

    void CalcTriangles()
    {
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    IEnumerator LoadRadius(Vector3 center, int r)
    {
        for (int i=-r; i<=r; i++)
        {
            for (int j=-r; j<=r; j++)
            {
                LoadChunk((int) center.x + i, (int) center.z + j);
                yield return null;//new WaitForSeconds(0.01f);
            }
        }

        for (int i=-(r+1); i<=r+1; i++)
        {
            UnloadChunk((int) center.x + i, (int) center.z + r + 1);
            UnloadChunk((int) center.x + i, (int) center.z - r - 1);
        }

        for (int j=-r; j<=r; j++)
        {
            UnloadChunk((int) center.x + r + 1, (int) center.z + j);
            UnloadChunk((int) center.x - r - 1, (int) center.z + j);
        }
    }

    void UnloadChunk(int x, int z)
    {
		Chunk chunk = chunks.Find((c) => c.GetPosition() == new Vector3(x, 0, z));
        if (chunk != null)
        {
            chunk.GetObj().SetActive(false);
        }
    }

    void LoadChunk(int x, int z)
    {
		Chunk chunk = chunks.Find((c) => c.GetPosition() == new Vector3(x, 0, z));
        if (chunk != null)
        {
            chunk.GetObj().SetActive(true);
        }
        else 
        {
			//Thread t = new Thread(new ParameterizedThreadStart(this.GenerateChunk));
			//t.Start((x, z));
            //Thread.Sleep(0);
            StartCoroutine(GenerateChunk(x, z));
        }
    }


    IEnumerator GenerateChunk(int xPos, int zPos)
    {
        GameObject chunkObj = new GameObject();

        chunkObj.SetActive(false);
        chunkObj.transform.position = new Vector3(xPos*xSize*vertFrequency, 0, zPos*zSize*vertFrequency);
        chunkObj.name = "Chunk (" + xPos + ", " + zPos + ")";
        chunkObj.gameObject.tag = "ground";
        chunkObj.transform.parent = transform;
        chunkObj.layer = 6;

        chunks.Add(new Chunk(xPos, zPos, chunkObj));

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        vertexColors = new Color[(xSize + 1) * (zSize + 1)];

        for (int z = 0, i = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = 45*Mathf.PerlinNoise(5000 - (0.01f*vertFrequency*(x+xPos*xSize)),  5000 - (0.01f*vertFrequency*(z+zPos*zSize)));//*biomeFactor[(Mathf.Abs(zPos)*200+Mathf.Abs(xPos)*20+i)%40000];
                vertices[i] = new Vector3(vertFrequency * x, y, vertFrequency * z);

                if (y > maxTerrainHeight) maxTerrainHeight = y;
                if (y < minTerrainHeight) minTerrainHeight = y;
                i++;
            }
        }


        for (int z = 0, j = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float normalizedHeight = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[j].y);
                vertexColors[j] = gradient.Evaluate(normalizedHeight);
                j++;
            }
        }

        Mesh mesh = new Mesh();
        MeshCollider meshCollider = new MeshCollider();
        MeshRenderer meshRenderer = new MeshRenderer();

        chunkObj.AddComponent<MeshFilter>();
        chunkObj.AddComponent<MeshCollider>();
        chunkObj.AddComponent<MeshRenderer>();

        mesh = chunkObj.GetComponent<MeshFilter>().mesh;
        meshCollider = chunkObj.GetComponent<MeshCollider>();
        meshRenderer = chunkObj.GetComponent<MeshRenderer>();
        meshRenderer.material = mat;

        UpdateMesh(mesh, meshCollider);
        chunkObj.SetActive(true);
        yield return null;
    }


    void UpdateMesh(Mesh mesh, MeshCollider meshCollider)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = vertexColors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;
    }

    // void GenerateNullObjects(Vector3[] vertices, int xPos, int zPos) // x and z pos are in chunks
    // {
    //     BuildUI buildUI = GameObject.FindGameObjectWithTag("buildmanager").GetComponent<BuildUI>() as BuildUI;
    //     for (int x = 0; x < 20; x++)
    //     {
    //         for (int z = 0; z < 20; z++)
    //         {
    //             float vertHeight = vertices[805 + 10*x + 1000*z].y;
    //             float stackHeight = 10*(vertHeight % 10 < 8 ? Mathf.FloorToInt(vertHeight / 10) : Mathf.CeilToInt(vertHeight / 10));
    //             GameObject[] objs = new GameObject[stackHeight == 0 ? 0 : Mathf.FloorToInt(stackHeight / 10) -1];

    //             for (int i = 0; i < (stackHeight / 10) -1; i++)
    //             {
    //                 GameObject nObj = new GameObject("NullObj (" + (xPos*200 + x*10) + ", " + stackHeight + ", " + (zPos*200 + z*10) + ")", typeof(NullObject));
    //                 objs[i] = nObj;
    //                 nObj.transform.parent = nullObjContainer.transform;
    //             }
    //             Stack stack = new Stack(xPos*200 + x*10, zPos*200 + z*10, (int)stackHeight-10, objs);
    //             buildUI.occupiedTiles.Add(stack);

    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= chunkUpdateTime)
        {
            Vector3 c = camScript.GetChunk();
            if (c != currOccupiedChunk)
            {
                currOccupiedChunk = c;
                if (enableChunkLoading) StartCoroutine(LoadRadius(c, 3));
            }
        }
    }

    public int GetVertFrequency()
    {
        return vertFrequency;
    }
}
