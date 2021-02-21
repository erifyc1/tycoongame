using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour, IActivatable, IConveyor
{
    public Vector2 getAcceleration(Vector3 position)
    {
        return new Vector2(transform.up.x, transform.up.z) * 5;
    }

    public void Activate()
    {

    }
    public void Deactivate()
    {

    }
}
