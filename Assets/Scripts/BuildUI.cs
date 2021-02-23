using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField]
    string[] objNames;

    [SerializeField]
    GameObject[] objPrefabs;

    [SerializeField]
    private List<Stack> occupiedTiles = new List<Stack>();

    Dictionary<string, GameObject> objMap = new Dictionary<string, GameObject>();
    public bool placingObject = false;
    GameObject currPlacingObject;
    private bool LMB = false;
    private Vector2 mousePos;

    private void Awake() {
        for (int i = 0; i < objPrefabs.Length; i++) {
            objMap.Add(objNames[i], objPrefabs[i]);
        }

        
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (placingObject && context.ReadValue<float>() == 1 && context.started && currPlacingObject.GetComponents<IRotatable>().Length > 0)
        {
            Rotate(true, currPlacingObject.GetComponents<IRotatable>()[0]);
        }

    }

    private void Rotate(bool right, IRotatable rot)
    {
        Direction newDir;
        Direction facing = rot.GetFacing();
        switch (facing)
        {
            case (Direction.NORTH):
                newDir = right ? Direction.EAST : Direction.WEST;
                break;
            case (Direction.EAST):
                newDir = right ? Direction.SOUTH : Direction.NORTH;
                break;
            case (Direction.SOUTH):
                newDir = right ? Direction.WEST : Direction.EAST;
                break;
            case (Direction.WEST):
                newDir = right ? Direction.NORTH : Direction.SOUTH;
                break;
            default:
                newDir = facing;
                break;
        }
        rot.SetDirection(newDir);
    }

    public void OnLMB(InputAction.CallbackContext context)
    {

        LMB = context.ReadValue<float>() == 0 ? false : true;
        if (LMB && placingObject)
        {
            Vector3 objPos = currPlacingObject.transform.position;
            if (occupiedTiles.Exists((stack) => stack.x == objPos.x && stack.y == objPos.z))
            {
                Stack tile = occupiedTiles.Find((stack) => stack.x == objPos.x && stack.y == objPos.z);

                if (!tile.capped)
                {
                    currPlacingObject.GetComponent<CommonProperties>().SetStackPos(tile.objs.Count);
                    tile.StackObject(currPlacingObject);
                    if (currPlacingObject.GetComponents<IActivatable>().Length != 0)
                    {
                        currPlacingObject.GetComponents<IActivatable>()[0].Activate();
                    }

                    Cursor.visible = true;
                    placingObject = false;
                    currPlacingObject = null;
                }
            }
            else
            {
                Stack stack = new Stack(objPos.x, objPos.z, (int) (objPos.y + 5), currPlacingObject);
                occupiedTiles.Add(stack);
                currPlacingObject.GetComponent<CommonProperties>().SetStackPos(stack.objs.Count);
                if (currPlacingObject.GetComponents<IActivatable>().Length != 0)
                {
                    currPlacingObject.GetComponents<IActivatable>()[0].Activate();
                }

                Cursor.visible = true;
                placingObject = false;
                currPlacingObject = null;
            }
        }

    }

    public void OnMouse(InputAction.CallbackContext context)
    {

        mousePos = context.ReadValue<Vector2>();

    }

    public void BuildObject(string name)
    {
        if (!placingObject && objMap[name] != null)
        {
            currPlacingObject = Instantiate(objMap[name]);
            placingObject = true;
        }
    }

    void OnGUI()
    {
        if (placingObject)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));

            float closestX = point.x % 10 < 5 ? Mathf.FloorToInt(point.x / 10) * 10 : Mathf.CeilToInt(point.x / 10) * 10;
            float closestZ = point.z % 10 < 5 ? Mathf.FloorToInt(point.z / 10) * 10 : Mathf.CeilToInt(point.z / 10) * 10;

            if (currPlacingObject.tag == "convVariant")
            {
                int stackPos = currPlacingObject.GetComponent<CommonProperties>().GetStackPos();
                Dictionary<string, GameObject> adjBlocks = GetAdjacentTiles(closestX, closestZ, stackPos);

                GameObject northBlock = adjBlocks.ContainsKey("north") ? adjBlocks["north"] : null;
                GameObject eastBlock = adjBlocks.ContainsKey("east") ? adjBlocks["east"] : null;
                GameObject southBlock = adjBlocks.ContainsKey("south") ? adjBlocks["south"] : null;
                GameObject westBlock = adjBlocks.ContainsKey("west") ? adjBlocks["west"] : null;

                bool northExists = northBlock != null && northBlock.tag == "convVariant";
                bool eastExists = eastBlock != null && eastBlock.tag == "convVariant";
                bool southExists = southBlock != null && southBlock.tag == "convVariant";
                bool westExists = westBlock != null && westBlock.tag == "convVariant";

                bool north = northExists && northBlock.GetComponent<CommonProperties>().GetFacing() == Direction.SOUTH;
                bool east = eastExists && eastBlock.GetComponent<CommonProperties>().GetFacing() == Direction.WEST;
                bool south = southExists && southBlock.GetComponent<CommonProperties>().GetFacing() == Direction.NORTH;
                bool west = westExists && westBlock.GetComponent<CommonProperties>().GetFacing() == Direction.EAST;

                string prefabName = Utils.GetDirection(currPlacingObject.GetComponent<CommonProperties>().GetFacing(), north, east, south, west);
                
                if (currPlacingObject.name != prefabName) {
                    Direction objDir = currPlacingObject.GetComponent<CommonProperties>().GetFacing();
                    Destroy(currPlacingObject);
                    currPlacingObject = Instantiate(objMap[prefabName], new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
                    currPlacingObject.GetComponent<CommonProperties>().SetDirection(objDir);
                }

                if (northExists) {
                    UpdateBlock(closestX, closestZ + 10, stackPos);
                }
                if (eastExists) {
                    UpdateBlock(closestX + 10, closestZ, stackPos);
                }
                if (southExists) {
                    UpdateBlock(closestX, closestZ - 10, stackPos);
                }
                if (westExists) {
                    UpdateBlock(closestX - 10, closestZ, stackPos);
                }
            }

            if (occupiedTiles.Exists((stack) => stack.x == closestX && stack.y == closestZ))
            {
                Stack stack = occupiedTiles.Find((stack) => stack.x == closestX && stack.y == closestZ);

                currPlacingObject.transform.position = new Vector3(closestX, stack.height + 10, closestZ);
            }
            else
            {
                currPlacingObject.transform.position = new Vector3(closestX, 0, closestZ);
            }
            Cursor.visible = false;
        }
    }

    private Dictionary<string, GameObject> GetAdjacentTiles(float x, float z, int stackPos)
    {
        Dictionary<string, GameObject> ret = new Dictionary<string, GameObject>();

        float cx = currPlacingObject.transform.position.x;
        float cz = currPlacingObject.transform.position.z;

        float closestX = cx % 10 < 5 ? Mathf.FloorToInt(cx / 10) * 10 : Mathf.CeilToInt(cx / 10) * 10;
        float closestZ = cz % 10 < 5 ? Mathf.FloorToInt(cz / 10) * 10 : Mathf.CeilToInt(cz / 10) * 10;

        bool inLayer = currPlacingObject.GetComponent<CommonProperties>().GetStackPos() == stackPos;
        
        if (occupiedTiles.Exists((stack) => stack.x == x + 10 && stack.y == z)) {
            ret.Add("east", occupiedTiles.Find((stack) => stack.x == x + 10 && stack.y == z).objs[stackPos]);
        }
        else if (inLayer && closestX == x + 10 && closestZ == z)
        {
            ret.Add("east", currPlacingObject);
            //Debug.Log("east");
        }

        if (occupiedTiles.Exists((stack) => stack.x == x - 10 && stack.y == z)) {
            ret.Add("west", occupiedTiles.Find((stack) => stack.x == x - 10 && stack.y == z).objs[stackPos]);
        }
        else if (inLayer && closestX == x - 10 && closestZ == z)
        {
            ret.Add("west", currPlacingObject);
            //Debug.Log("west");
        }

        if (occupiedTiles.Exists((stack) => stack.x == x && stack.y == z - 10)) {
            ret.Add("south", occupiedTiles.Find((stack) => stack.x == x && stack.y == z - 10).objs[stackPos]);
        }
        else if (inLayer && closestX == x && closestZ == z - 10)
        {
            ret.Add("south", currPlacingObject);
            //Debug.Log("south");
        }

        if (occupiedTiles.Exists((stack) => stack.x == x && stack.y == z + 10)) {
            ret.Add("north", occupiedTiles.Find((stack) => stack.x == x && stack.y == z + 10).objs[stackPos]);
        }
        else if (inLayer && closestX == x + 10 && closestZ == z + 10)
        {
            ret.Add("north", currPlacingObject);
            //Debug.Log("north");
        }
        foreach (string s in ret.Keys) {
            Debug.Log(s);
        }
        return ret;
    }

    private void UpdateBlock(float x, float z, int stackPos) {
        if (occupiedTiles.Exists((stack) => stack.x == x && stack.y == z)) {
            Dictionary<string, GameObject> adjBlocks = GetAdjacentTiles(x, z, stackPos);
            Stack targetStack = occupiedTiles.Find((stack) => stack.x == x && stack.y == z);
            GameObject targetBlock = targetStack.objs[stackPos];

            GameObject northBlock = adjBlocks.ContainsKey("north") ? adjBlocks["north"] : null;
            GameObject eastBlock = adjBlocks.ContainsKey("east") ? adjBlocks["east"] : null;
            GameObject southBlock = adjBlocks.ContainsKey("south") ? adjBlocks["south"] : null;
            GameObject westBlock = adjBlocks.ContainsKey("west") ? adjBlocks["west"] : null;

            bool northExists = northBlock != null && northBlock.tag == "convVariant";
            bool eastExists = eastBlock != null && eastBlock.tag == "convVariant";
            bool southExists = southBlock != null && southBlock.tag == "convVariant";
            bool westExists = westBlock != null && westBlock.tag == "convVariant";

            bool north = northExists && northBlock.GetComponent<CommonProperties>().GetFacing() == Direction.SOUTH;
            bool east = eastExists && eastBlock.GetComponent<CommonProperties>().GetFacing() == Direction.WEST;
            bool south = southExists && southBlock.GetComponent<CommonProperties>().GetFacing() == Direction.NORTH;
            bool west = westExists && westBlock.GetComponent<CommonProperties>().GetFacing() == Direction.EAST;

            string prefabName = Utils.GetDirection(targetBlock.GetComponent<CommonProperties>().GetFacing(), north, east, south, west);
            foreach (string s in adjBlocks.Keys) {
                Debug.Log(s);
            }
            //Debug.Log(new Vector2(northExists ? 1 : 0, southExists ? 1 : 0));
            //Debug.Log(new Vector2(eastExists ? 1 : 0, westExists ? 1 : 0));
            //Debug.Log(prefabName);

            if (targetBlock.name != prefabName) {
                Direction objDir = targetBlock.GetComponent<CommonProperties>().GetFacing();
                Destroy(targetBlock);
                targetStack.objs[stackPos] = Instantiate(objMap[prefabName], new Vector3(x, stackPos * 10, z), Quaternion.Euler(0, 0, 0));
                targetStack.objs[stackPos].GetComponent<CommonProperties>().SetDirection(objDir);
            }
        }
    }
}
