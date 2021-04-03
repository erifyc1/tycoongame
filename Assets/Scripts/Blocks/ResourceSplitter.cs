using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSplitter : ResourcePusher
{
    [SerializeField] ConveyorSettings rightSettings;
    [SerializeField] ConveyorSettings leftSettings;

    private List<GameObject> sortRight = new List<GameObject>();
    private List<GameObject> sortLeft = new List<GameObject>();

    private bool lastResourceRight = false;

    private CommonProperties common;


    void Start()
    {
        common = transform.parent.GetComponent<CommonProperties>();
        if (rightSettings == null || leftSettings == null) throw new MissingReferenceException("missing conveyor settings");
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            if (lastResourceRight)
            {
                sortLeft.Add(hit);
            }
            else
            {
                sortRight.Add(hit);
            }
            lastResourceRight = !lastResourceRight;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            if (sortRight.Contains(hit))
            {
                sortRight.Remove(hit);
            }
            if (sortLeft.Contains(hit))
            {
                sortLeft.Remove(hit);
            }
        }
    }


    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        PushResources(sortRight, rightSettings, dt);
        PushResources(sortLeft, leftSettings, dt);
    }
}
