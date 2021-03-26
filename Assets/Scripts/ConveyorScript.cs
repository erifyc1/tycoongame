using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour, IActivatable, IConveyor
{
    [SerializeField] Conveyor pushDirection = Conveyor.forward;

    private enum Conveyor
    {
        forward,
        left,
        right
    }

    public Vector2 getAcceleration(Vector3 position)
    {
        if (pushDirection == Conveyor.forward)
        {
            return new Vector2(transform.up.x, transform.up.z) * 5;
        }
        else if (pushDirection == Conveyor.left)
        {
            return new Vector2(transform.right.x, transform.right.z) * 5; // left and right are same relative directin due to rotation of the prefabs
        }
        else if (pushDirection == Conveyor.right)
        {
            return new Vector2(transform.right.x, transform.right.z) * 5;
        }
        else return Vector3.zero;
    }

    public void Activate()
    {

    }
    public void Deactivate()
    {

    }
}
