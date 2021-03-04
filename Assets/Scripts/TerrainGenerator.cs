using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // [SerializeField]
    // GameObject newTerrainBlock;

    // private MeshFilter mf;
    // private List<Vector3> gridmap = new List<Vector3>();
    // private GameObject nullObjContainer;

    // [SerializeField]
    // int xBlocks = 0;
    // [SerializeField]
    // int zBlocks = 0;

    // void Start()
    // {
    //     nullObjContainer = GameObject.FindGameObjectWithTag("nObjContainer");
    //     mf = transform.GetComponent<MeshFilter>();

    //     for (int z = 0; z < zBlocks; z++)
    //     {
    //         for (int x = 0; x < xBlocks; x++)
    //         {
    //             float h = Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 50f;
    //             int y = h % 10 < 5 ? Mathf.FloorToInt(h / 10) : Mathf.CeilToInt(h / 10);
    //             gridmap.Add(10 * new Vector3(x, y, z));
    //             //Instantiate(newTerrainBlock, new Vector3(x,y,z)*10, Quaternion.identity);
    //         }
    //     }

    //     for (int i = 0; i < gridmap.Count; i++)
    //     {
    //         NewBlock(gridmap[i]);
    //     }

    //     Vector3 bottomLeft = new Vector3(-xBlocks * 5, -5, -zBlocks * 5);
    //     transform.position = bottomLeft;
    //     MeshCollider mc = this.gameObject.AddComponent<MeshCollider>();
    // }

    // // Update is called once per frame
    // void Update() { }
    // void NewBlock(Vector3 blockPos)
    // {
    //     //Debug.Log("blockPos.y: " + blockPos.y);
    //     BuildUI buildUI = GameObject.FindGameObjectWithTag("buildmanager").GetComponent<BuildUI>() as BuildUI;
    //     GameObject[] objs = new GameObject[blockPos.y == 0 ? 0 : Mathf.FloorToInt(blockPos.y / 10) - 1];

    //     for (int i = 0; i < blockPos.y / 10 - 1; i++)
    //     {
    //         GameObject nObj = new GameObject("nullobj", typeof(NullObject));
    //         objs[i] = nObj;
    //         nObj.transform.parent = nullObjContainer.transform;
    //     }
    //     Stack stack = new Stack(-xBlocks * 5 + blockPos.x, -zBlocks * 5 + blockPos.z, (int)blockPos.y - 10, objs);
    //     buildUI.occupiedTiles.Add(stack);
    //     GameObject block = Instantiate(newTerrainBlock, blockPos, Quaternion.identity);
    //     block.transform.parent = transform;
    //     Combine(block);
    // }

    // void Combine(GameObject block)
    // {
    //     MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    //     CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    //     Destroy(this.gameObject.GetComponent<MeshCollider>());

    //     int i = 0;

    //     while (i < meshFilters.Length)
    //     {
    //         combine[i].mesh = meshFilters[i].sharedMesh;
    //         combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //         meshFilters[i].gameObject.SetActive(false);
    //         i++;
    //     }

    //     mf.mesh = new Mesh();
    //     mf.mesh.CombineMeshes(combine, true);
    //     transform.localScale = new Vector3(1, 1, 1);
    //     mf.mesh.RecalculateBounds();
    //     mf.mesh.RecalculateNormals();
    //     mf.mesh.Optimize();
    //     transform.gameObject.SetActive(true);

    //     Destroy(block);
    // }
}
