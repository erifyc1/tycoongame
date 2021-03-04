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

    Dictionary<string, GameObject> objMap = new Dictionary<string, GameObject>(); // dictionary of placable objects
    private string streakPlaceName = "";

    public WorldGrid world;

    public bool placingObject = false;
    private bool snapped = false;
    
    private GameObject currPlacingObject;
    private Vector3 placingObjPos = Vector3.zero;
    private Vector3 lastKnownPos = Vector3.zero;

    bool LMB = false;

    private Vector2 mousePos;
    private bool raycastPlace = true;

    private void Awake() {
        world = new WorldGrid();

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
        LMB = context.ReadValue<float>() == 1;

        if (snapped && LMB && context.started && placingObject && !(world.GridSpaceExists(placingObjPos)))
        {

            if (currPlacingObject.GetComponents<IActivatable>().Length != 0)
            {
                currPlacingObject.GetComponents<IActivatable>()[0].Activate();
            }

            placingObject = false;
            currPlacingObject.layer = 7;
            world.AddSpace(new GridSpace(placingObjPos, currPlacingObject));
            currPlacingObject.GetComponent<CommonProperties>().SetTransparency(1f, BlendMode.Opaque);
            currPlacingObject = null;

            if (streakPlaceName != "")
            {
                BuildObject(streakPlaceName, true);
            }
        }

    }

    public void OnMouse(InputAction.CallbackContext context)
    {

        mousePos = context.ReadValue<Vector2>();

    }

    public void BuildObject(string name, bool auto) // auto is true if BuildObject() was triggered by placing an object (on streak)
    {
        if (auto)
        {
            Build(name);
        }
        else if (streakPlaceName == "")
        {
            streakPlaceName = name;
            Build(name);
            
        }
        else
        {
            if (currPlacingObject != null)
            {
                Destroy(currPlacingObject);
                currPlacingObject = null;
                placingObject = false;
            }
            streakPlaceName = "";
        }


    }
    private void Build(string name) // only to be invoked by BuildObject()
    {
        if (!placingObject && objMap[name] != null)
        {
            currPlacingObject = Instantiate(objMap[name]);
            currPlacingObject.name = name;
            currPlacingObject.GetComponent<CommonProperties>().SetTransparency(0.2f, BlendMode.Transparent);


            placingObject = true;
        }
    }


    void OnGUI()
    {
        snapped = false;
        if (placingObject)
        {

            if (raycastPlace)
            {
                var ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 90f, LayerMask.GetMask("object", "ground"), QueryTriggerInteraction.Collide))
                {
                    Vector3 hitLocation = ray.GetPoint(hit.distance) + hit.normal;

                    float x = 10 * (hitLocation.x % 10 < 5 ? Mathf.FloorToInt(hitLocation.x / 10) : Mathf.CeilToInt(hitLocation.x / 10));
                    float y = 10 * (hitLocation.y % 10 < 5 ? Mathf.FloorToInt(hitLocation.y / 10) : Mathf.CeilToInt(hitLocation.y / 10));
                    float z = 10 * (hitLocation.z % 10 < 5 ? Mathf.FloorToInt(hitLocation.z / 10) : Mathf.CeilToInt(hitLocation.z / 10));

                    if (!world.GridSpaceExists(new Vector3(x, y, z)))
                    {
                        placingObjPos = new Vector3(x, y, z);
                        snapped = true;
                    }
                }
                else
                {
                    placingObjPos = ray.GetPoint(90f);
                }
            }
            // else alternate camera mode
            // {
            //     Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));

            //     closestX = point.x % 10 < 5 ? Mathf.FloorToInt(point.x / 10) * 10 : Mathf.CeilToInt(point.x / 10) * 10;
            //     closestZ = point.z % 10 < 5 ? Mathf.FloorToInt(point.z / 10) * 10 : Mathf.CeilToInt(point.z / 10) * 10;
            // }

            if (currPlacingObject.tag == "convVariant" && snapped)
            {
                UpdateBlock(placingObjPos, true);
                
                UpdateBlock(placingObjPos + 10 * Vector3.forward, false);
                UpdateBlock(placingObjPos + 10 * Vector3.right, false);
                UpdateBlock(placingObjPos - 10 * Vector3.forward, false);
                UpdateBlock(placingObjPos - 10 * Vector3.right, false);

                if (lastKnownPos != placingObjPos)
                {
                    UpdateBlock(lastKnownPos + 10 * Vector3.forward, false, true);
                    UpdateBlock(lastKnownPos + 10 * Vector3.right, false, true);
                    UpdateBlock(lastKnownPos - 10 * Vector3.forward, false, true);
                    UpdateBlock(lastKnownPos - 10 * Vector3.right, false, true);
                }

                lastKnownPos = placingObjPos;
            }

            currPlacingObject.transform.position = placingObjPos;
        }
    }

    private void UpdateBlock(Vector3 position, bool currPlacingAtPosition) // default ignore = false
    {
        UpdateBlock(position, currPlacingAtPosition, false);
    }

    private void UpdateBlock(Vector3 position, bool currPlacingAtPosition, bool ignoreCurrPlacing)
    {
        if (ignoreCurrPlacing) currPlacingAtPosition = false;

        if (world.GridSpaceExists(position) || currPlacingAtPosition) {
			Dictionary<string, GridSpace> adjBlocks = world.FindAdjacentOccupied(position);

            GridSpace targetSpace = world.FindNearestGridSpace(position); // would be null if currplacingatposition
            GameObject targetBlock;

            if (currPlacingAtPosition)
            { 
                targetBlock = currPlacingObject;
            }
            else 
            {
                targetBlock = targetSpace.GetObject();
            }

            if (targetBlock.tag == "convVariant")
            {
                GameObject northBlock = adjBlocks.ContainsKey("north") ? adjBlocks["north"].GetObject() : null;
                GameObject eastBlock = adjBlocks.ContainsKey("east") ? adjBlocks["east"].GetObject() : null;
                GameObject southBlock = adjBlocks.ContainsKey("south") ? adjBlocks["south"].GetObject() : null;
                GameObject westBlock = adjBlocks.ContainsKey("west") ? adjBlocks["west"].GetObject() : null;

                if (!currPlacingAtPosition && !ignoreCurrPlacing)
                {
                    Vector3 relative = position-placingObjPos;
                    if (relative.x == -10)
                    {
                        eastBlock = currPlacingObject;
                    }
                    else if (relative.x == 10)
                    {
                        westBlock = currPlacingObject;
                    }

                    if (relative.z == -10)
                    {
                        northBlock = currPlacingObject;
                    }
                    else if (relative.z == 10)
                    {
                        southBlock = currPlacingObject;
                    }
                }

                bool northExists = northBlock != null && northBlock.tag == "convVariant";
                bool eastExists = eastBlock != null && eastBlock.tag == "convVariant";
                bool southExists = southBlock != null && southBlock.tag == "convVariant";
                bool westExists = westBlock != null && westBlock.tag == "convVariant";

                bool north = northExists && northBlock.GetComponent<CommonProperties>().GetFacing() == Direction.SOUTH;
                bool east = eastExists && eastBlock.GetComponent<CommonProperties>().GetFacing() == Direction.WEST;
                bool south = southExists && southBlock.GetComponent<CommonProperties>().GetFacing() == Direction.NORTH;
                bool west = westExists && westBlock.GetComponent<CommonProperties>().GetFacing() == Direction.EAST;

                //if (north || south || east || west) Debug.Log("test");

                string prefabName = Utils.GetDirection(targetBlock.GetComponent<CommonProperties>().GetFacing(), north, east, south, west);

                if (targetBlock.name != prefabName) {
                    Direction objDir = targetBlock.GetComponent<CommonProperties>().GetFacing();
                    int objLayer = targetBlock.layer;

                    if (currPlacingAtPosition)
                    {
                        Destroy(currPlacingObject);
                        currPlacingObject = Instantiate(objMap[prefabName], new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
                        currPlacingObject.GetComponent<CommonProperties>().SetDirection(objDir);
                        currPlacingObject.layer = objLayer;
                        currPlacingObject.name = prefabName;
                        currPlacingObject.GetComponent<CommonProperties>().SetTransparency(0.2f, BlendMode.Transparent);
                    }
                    else
                    {
                        world.DeleteSpace(targetSpace);
                        GameObject replObj = Instantiate(objMap[prefabName]);
                        replObj.transform.position = position;
                        replObj.name = prefabName;

                        GridSpace replacement = new GridSpace(position, replObj);
                        world.AddSpace(replacement);
                        replacement.GetObject().GetComponent<CommonProperties>().SetDirection(objDir);
                        replacement.GetObject().layer = objLayer;
                    }
                }
            }
        }
    }


    public void on1(InputAction.CallbackContext context)
    {
        BuildObject("cube", false);
    }

    public void on2(InputAction.CallbackContext context)
    {
        BuildObject("conveyor", false);
    }

    public void on3(InputAction.CallbackContext context)
    {
        BuildObject("generator", false);
    }
    
    public void on4(InputAction.CallbackContext context)
    {
        
    }

    public void on5(InputAction.CallbackContext context)
    {

    }

    public void on6(InputAction.CallbackContext context)
    {
        
    }
    
    public void on7(InputAction.CallbackContext context)
    {

    }
    
    public void on8(InputAction.CallbackContext context)
    {
        
    }

    public void on9(InputAction.CallbackContext context)
    {

    }
    
    public void on0(InputAction.CallbackContext context)
    {
        
    }
}
