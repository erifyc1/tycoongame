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
        Debug.Log(context.ReadValue<float>());
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
                GameObject northBlock = null;
                GameObject westBlock = null;
                GameObject southBlock = null;
                GameObject eastBlock = null;

                int stackPos = currPlacingObject.GetComponent<CommonProperties>().GetStackPos();

                if (occupiedTiles.Exists((stack) => stack.x == closestX + 10 && stack.y == closestZ)) {
                    eastBlock = occupiedTiles.Find((stack) => stack.x == closestX + 10 && stack.y == closestZ).objs[stackPos];
                }
                if (occupiedTiles.Exists((stack) => stack.x == closestX - 10 && stack.y == closestZ)) {
                    westBlock = occupiedTiles.Find((stack) => stack.x == closestX - 10 && stack.y == closestZ).objs[stackPos];
                }
                if (occupiedTiles.Exists((stack) => stack.x == closestX && stack.y == closestZ - 10)) {
                    southBlock = occupiedTiles.Find((stack) => stack.x == closestX && stack.y == closestZ - 10).objs[stackPos];
                }
                if (occupiedTiles.Exists((stack) => stack.x == closestX && stack.y == closestZ + 10)) {
                    northBlock = occupiedTiles.Find((stack) => stack.x == closestX && stack.y == closestZ + 10).objs[stackPos];
                }
                
                bool north = northBlock != null && northBlock.tag == "convVariant" && northBlock.GetComponent<CommonProperties>().GetFacing() == Direction.SOUTH;
                bool west = westBlock != null && westBlock.tag == "convVariant" && westBlock.GetComponent<CommonProperties>().GetFacing() == Direction.EAST;
                bool south = southBlock != null && southBlock.tag == "convVariant" && southBlock.GetComponent<CommonProperties>().GetFacing() == Direction.NORTH;
                bool east = eastBlock != null && eastBlock.tag == "convVariant" && eastBlock.GetComponent<CommonProperties>().GetFacing() == Direction.WEST;

                string prefabName = Utils.GetDirection(currPlacingObject.GetComponent<CommonProperties>().GetFacing(), north, east, south, west);
                
                if (currPlacingObject.name != prefabName) {
                    Direction objDir = currPlacingObject.GetComponent<CommonProperties>().GetFacing();
                    Destroy(currPlacingObject);
                    currPlacingObject = Instantiate(objMap[prefabName], new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
                    currPlacingObject.GetComponent<CommonProperties>().SetDirection(objDir);
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
}
