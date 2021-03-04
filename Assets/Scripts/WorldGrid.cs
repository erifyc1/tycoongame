using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid
{
    private List<GridSpace> occupiedGridSpaces = new List<GridSpace>();

    public WorldGrid()
    {

    }

    public bool GridSpaceExists(Vector3 position)
    {
        float x = 10 * (position.x % 10 < 5 ? Mathf.FloorToInt(position.x / 10) : Mathf.CeilToInt(position.x / 10));
        float y = 10 * (position.y % 10 < 5 ? Mathf.FloorToInt(position.y / 10) : Mathf.CeilToInt(position.y / 10));
        float z = 10 * (position.z % 10 < 5 ? Mathf.FloorToInt(position.z / 10) : Mathf.CeilToInt(position.z / 10));
        return occupiedGridSpaces.Exists((space) => space.GetPosition() == new Vector3(x, y, z));
    }

    public GridSpace FindNearestGridSpace(Vector3 position) // finds closest grid space given coordinates, return null if not
    {
        if (GridSpaceExists(position))
        {
            float x = 10 * (position.x % 10 < 5 ? Mathf.FloorToInt(position.x / 10) : Mathf.CeilToInt(position.x / 10));
            float y = 10 * (position.y % 10 < 5 ? Mathf.FloorToInt(position.y / 10) : Mathf.CeilToInt(position.y / 10));
            float z = 10 * (position.z % 10 < 5 ? Mathf.FloorToInt(position.z / 10) : Mathf.CeilToInt(position.z / 10));
            return occupiedGridSpaces.Find((space) => space.GetPosition() == new Vector3(x, y, z));
        }
        return null;
    }

    public void DeleteSpace(GridSpace g)
    {
        if (GridSpaceExists(g.GetPosition()))
        {
            g.DestroyGameObject();
            occupiedGridSpaces.Remove(g);
        }
    }

    public void AddSpace(GridSpace g)
    {
        occupiedGridSpaces.Add(g);
    }

    public Dictionary<string, GridSpace> FindAdjacentOccupied(Vector3 position) // add overload for currplacingobject case!!!!!
    {
        float x = 10 * (position.x % 10 < 5 ? Mathf.FloorToInt(position.x / 10) : Mathf.CeilToInt(position.x / 10));
        float y = 10 * (position.y % 10 < 5 ? Mathf.FloorToInt(position.y / 10) : Mathf.CeilToInt(position.y / 10));
        float z = 10 * (position.z % 10 < 5 ? Mathf.FloorToInt(position.z / 10) : Mathf.CeilToInt(position.z / 10));
        Dictionary<string, GridSpace> adjSpaces = new Dictionary<string, GridSpace>();

        adjSpaces.Add("east", FindNearestGridSpace(new Vector3(x + 10, y, z)));
        adjSpaces.Add("west", FindNearestGridSpace(new Vector3(x - 10, y, z)));
        adjSpaces.Add("up", FindNearestGridSpace(new Vector3(x, y + 10, z)));
        adjSpaces.Add("down", FindNearestGridSpace(new Vector3(x, y - 10, z)));
        adjSpaces.Add("north", FindNearestGridSpace(new Vector3(x, y, z + 10)));
        adjSpaces.Add("south", FindNearestGridSpace(new Vector3(x, y, z - 10)));

        Dictionary<string, GridSpace> adjSpacesTrimmed = new Dictionary<string, GridSpace>();
        foreach (KeyValuePair<string, GridSpace> entry in adjSpaces)
        {
            //Debug.Log(entry);
            if (entry.Value != null)
            {
                adjSpacesTrimmed.Add(entry.Key, entry.Value);
            }
        }

        return adjSpacesTrimmed;
    }
}
