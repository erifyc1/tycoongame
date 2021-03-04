using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScriptLeft : MonoBehaviour, IActivatable, IConveyor
{
    public Vector2 getAcceleration(Vector3 position)
    {
        Vector3 localPosition = new Vector3(transform.position.x - position.x, transform.position.y - position.y, transform.position.z - position.z);
        int tx = Mathf.FloorToInt(transform.up.x + 0.5f);
        int tz = Mathf.FloorToInt(transform.up.z + 0.5f); //left up is forward
        float Lx = localPosition.x;
        float Lz = localPosition.z;
        int x = tx - tz;
        int z = tx + tz;
        Vector2 accel = new Vector2(Mathf.Pow(2, -Mathf.Abs(tx)) * (5 + (Lz * tz + Lx * -tx) * x) * x, Mathf.Pow(2, -Mathf.Abs(tz)) * (5 + (Lz * -tz + Lx * tx) * z) * z);
        //Debug.Log(new Vector2(tx, tz));
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
