using UnityEngine;

public class Utils
{
    public static string GetDirection(Direction orientation, bool north, bool east, bool south, bool west)
    {
        bool left = orientation switch
        {
            Direction.NORTH => west,
            Direction.EAST => north,
            Direction.SOUTH => east,
            Direction.WEST => south,
            _ => false
        };
        bool back = orientation switch
        {
            Direction.NORTH => south,
            Direction.EAST => west,
            Direction.SOUTH => north,
            Direction.WEST => east,
            _ => false
        };
        bool right = orientation switch
        {
            Direction.NORTH => east,
            Direction.EAST => south,
            Direction.SOUTH => west,
            Direction.WEST => north,
            _ => false
        };

        if (!left && !right)
        {
            return "conveyor";
        }
        else if (!back)
        {
            if (!left && right)
            {
                return "conv_right";
            }
            else if (left && !right)
            {
                return "conv_left";
            }
            else if (left && right)
            {
                return "conv_m_both";
            }
        }
        else if (back)
        {
            if (!left && right)
            {
                return "conv_m_right";
            }
            else if (left && !right)
            {
                return "conv_m_left";
            }
            else if (left && right)
            {
                return "conv_m_full";
            }
        }
        Debug.Log("shit happened it didnt work");
        return "fuck";
    }
}



