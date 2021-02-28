using UnityEngine;

public class Chunk
{
    private int x;
    private int z;
    private GameObject meshObj;
    public Chunk(int x, int z, GameObject mesh)
    {
        this.x = x;
        this.z = z;
        meshObj = mesh;
    }

    public GameObject GetObj()
    {
        return meshObj;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, 0, z);
    }
}
