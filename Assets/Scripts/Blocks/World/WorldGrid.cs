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
        Vector3 gridPosition = Utils.RoundToNearestTen(position);
        return occupiedGridSpaces.Exists((space) => space.GetPosition() == gridPosition);
    }

    public GridSpace FindNearestGridSpace(Vector3 position) // finds closest grid space given coordinates, return null if not
    {
        if (GridSpaceExists(position))
        {
            Vector3 gridPosition = Utils.RoundToNearestTen(position);
            return occupiedGridSpaces.Find((space) => space.GetPosition() == gridPosition);
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
        Vector3 centerBlock = Utils.RoundToNearestTen(position);
        Dictionary<string, GridSpace> adjSpaces = new Dictionary<string, GridSpace>();

        adjSpaces.Add("east",  FindNearestGridSpace(new Vector3(centerBlock.x + 10, centerBlock.y,      centerBlock.z)));
        adjSpaces.Add("west",  FindNearestGridSpace(new Vector3(centerBlock.x - 10, centerBlock.y,      centerBlock.z)));
        adjSpaces.Add("up",    FindNearestGridSpace(new Vector3(centerBlock.x,      centerBlock.y + 10, centerBlock.z)));
        adjSpaces.Add("down",  FindNearestGridSpace(new Vector3(centerBlock.x,      centerBlock.y - 10, centerBlock.z)));
        adjSpaces.Add("north", FindNearestGridSpace(new Vector3(centerBlock.x,      centerBlock.y,      centerBlock.z + 10)));
        adjSpaces.Add("south", FindNearestGridSpace(new Vector3(centerBlock.x,      centerBlock.y,      centerBlock.z - 10)));

        Dictionary<string, GridSpace> adjSpacesTrimmed = new Dictionary<string, GridSpace>();
        foreach (KeyValuePair<string, GridSpace> entry in adjSpaces)
        { 
            if (entry.Value != null)
            {
                adjSpacesTrimmed.Add(entry.Key, entry.Value);
            }
        }

        return adjSpacesTrimmed;
    }
}
