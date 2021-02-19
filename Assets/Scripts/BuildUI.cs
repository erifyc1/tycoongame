using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectPrefabs;
    [SerializeField]
    private List<Stack> occupiedTiles = new List<Stack>();
    public bool placingObject = false;
    GameObject currPlacingObject;
    private bool rKey = false;
    private bool LMB = false;
    private Vector2 mousePos;

    public void OnRotate(InputAction.CallbackContext context)
    {

        rKey = context.ReadValue<float>() == 0 ? false : true;

    }

    public void OnLMB(InputAction.CallbackContext context)
    {

        LMB = context.ReadValue<float>() == 0 ? false : true;

    }

    public void OnMouse(InputAction.CallbackContext context)
    {

        mousePos = context.ReadValue<Vector2>();

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (placingObject)
        {
            Vector3 objPos = currPlacingObject.transform.position;
            //if (Input.GetKeyDown("r")) currPlacingObject.transform.Rotate(0, 0, 90);

            if (LMB)
            {
                if (occupiedTiles.Exists((stack) => stack.x == objPos.x && stack.y == objPos.z))
                {
                    Stack tile = occupiedTiles.Find((stack) => stack.x == objPos.x && stack.y == objPos.z);

                    if (!tile.capped)
                    {
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
                    Stack stack = new Stack(objPos.x, objPos.z, Mathf.FloorToInt(objPos.y / 10), currPlacingObject);
                    occupiedTiles.Add(stack);
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
    }

    public void BuildObject(int id)
    {
        if (!placingObject && objectPrefabs[id] != null)
        {
            currPlacingObject = Instantiate(objectPrefabs[id], new Vector3(0, 0, 100), Quaternion.Euler(0, 180, 0));
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
