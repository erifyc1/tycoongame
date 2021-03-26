using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonProperties : MonoBehaviour, IRotatable
{
    [SerializeField] Direction facing = Direction.NORTH;
    [SerializeField] List<Vector3> footPrintCoords = new List<Vector3>();

    private List<Color> defaultColors = new List<Color>();


    public Direction GetFacing()
    {
        return facing;
    }

    public List<Vector3> GetFootprint()
    {
        return footPrintCoords;
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

    public void InitMats()
    {
        if (TryGetComponent(typeof(MeshRenderer), out Component comp)) // init defaultMaterials
        {
            Material mat = comp.GetComponent<MeshRenderer>().material;
            defaultColors.Add(mat.color);
        }

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "ignoreTransparency" && transform.GetChild(i).TryGetComponent(typeof(MeshRenderer), out Component com))
                {
                    Material mat = com.GetComponent<MeshRenderer>().material;
                    defaultColors.Add(mat.color);
                }
            }
        }
    }

    public void SetTransparency(float alpha, BlendMode blendMode)
    {
        if (TryGetComponent(typeof(MeshRenderer), out Component comp))
        {
            Material mat = comp.GetComponent<MeshRenderer>().material;
            Utils.SetupMaterialWithBlendMode(mat, blendMode);
            Color c = mat.color;
            c.a = alpha;
            mat.color = c;
        }

        if (transform.childCount > 0)
        {
            for (int i=0; i<transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "ignoreTransparency" && transform.GetChild(i).TryGetComponent(typeof(MeshRenderer), out Component com))
                {
                    Material mat = com.GetComponent<MeshRenderer>().material;
                    Utils.SetupMaterialWithBlendMode(mat, blendMode);
                    Color c = mat.color;
                    c.a = alpha;
                    mat.color = c;
                }
            }
        }
    }



    public void SetTintColor(Color color)
    {
        if (TryGetComponent(typeof(MeshRenderer), out Component comp))
        {
            Material mat = comp.GetComponent<MeshRenderer>().material;
            Color c = mat.color;
            float alpha = c.a;

            c = color;
            c.a = alpha;
            mat.color = c;
        }

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "ignoreTransparency" && transform.GetChild(i).TryGetComponent(typeof(MeshRenderer), out Component com))
                {
                    Material mat = com.GetComponent<MeshRenderer>().material;
                    Color c = mat.color;
                    float alpha = c.a;

                    c = color;
                    c.a = alpha;
                    mat.color = c;
                }
            }
        }
    }

    public void ResetTintColor()
    {
        int idx = 0;
        if (TryGetComponent(typeof(MeshRenderer), out Component comp))
        {
            Material mat = comp.GetComponent<MeshRenderer>().material;
            float alpha = mat.color.a;

            Color original = defaultColors[idx];
            original.a = alpha;

            mat.color = original;
            idx++;
        }

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "ignoreTransparency" && transform.GetChild(i).TryGetComponent(typeof(MeshRenderer), out Component com))
                {
                    Material mat = com.GetComponent<MeshRenderer>().material;
                    float alpha = mat.color.a;

                    Color original = defaultColors[idx];
                    original.a = alpha;

                    mat.color = original;
                    idx++;
                }
            }
        }
    }


    public void SetColliderEnabled(bool enable)
    {
        if (TryGetComponent(typeof(Collider), out Component comp))
        {
            comp.GetComponent<Collider>().enabled = enable;
        }

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(typeof(Collider), out Component com))
                {
                    com.GetComponent<Collider>().enabled = enable;
                }
            }
        }
    }


}
