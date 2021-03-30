using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScriptRight : MonoBehaviour, IActivatable
{
    public Vector2 getAcceleration(Vector3 position)
    {
        Vector3 localPosition = new Vector3(transform.position.x - position.x, transform.position.y - position.y, transform.position.z - position.z);
        int tx = Mathf.FloorToInt(-transform.right.x + 0.5f);
        int tz = Mathf.FloorToInt(-transform.right.z + 0.5f); //right up is left
        float Lx = localPosition.x;
        float Lz = localPosition.z;
        int x = tx + tz;
        int z = tz - tx;
        Vector2 accel = new Vector2(Mathf.Pow(2, -Mathf.Abs(tx)) * (5 + (Lz * tz + Lx * -tx) * x) * x, Mathf.Pow(2, -Mathf.Abs(tx)) * (5 + (Lz * -tz + Lx * tx) * z) * z);
        //Debug.Log(tz);
        //Debug.Log(accel);
        return accel;
    }

    public void Activate()
    {

    }
    public void Deactivate()
    {

    }
}
