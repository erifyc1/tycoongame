using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonProperties : MonoBehaviour, IRotatable
{
    [SerializeField]
    Direction facing = Direction.NORTH;
    private int stackPos = 0;

    public Direction GetFacing()
    {
        return facing;
    }

    public void SetStackPos(int stackPos)
    {
        this.stackPos = stackPos;
    }

    public int GetStackPos()
    {
        return this.stackPos;
    }

    public void SetDirection(Direction dir)
    {
        facing = dir;
        int yDegrees;
        switch (dir)
        {
            case (Direction.NORTH):
                yDegrees = 0;
                break;
            case (Direction.EAST):
                yDegrees = 90;
                break;
            case (Direction.SOUTH):
                yDegrees = 180;
                break;
            case (Direction.WEST):
                yDegrees = 270;
                break;
            default:
                yDegrees = 0;
                Debug.Log("Bad direction");
                break;
        }
        transform.rotation = Quaternion.AngleAxis(yDegrees, Vector3.up);
    }
}
