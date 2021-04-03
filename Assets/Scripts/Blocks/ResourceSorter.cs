using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSorter : ResourcePusher
{
    [SerializeField] ConveyorSettings passthroughConvSettings;
    [SerializeField] ConveyorSettings sortConvSettings;

    [SerializeField] ResourceType sortType;

    private List<GameObject> passthroughResources = new List<GameObject>();
    private List<GameObject> sortResources = new List<GameObject>();


    private CommonProperties common;


    void Start()
    {
        common = transform.parent.GetComponent<CommonProperties>();
        if (passthroughConvSettings == null || sortConvSettings == null) throw new MissingReferenceException("missing conveyor settings");
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            if (hit.GetComponent<ResourceItem>().GetResourceType() == sortType)
            {
                sortResources.Add(hit);
            }
            else
            {
                passthroughResources.Add(hit);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            if (sortResources.Contains(hit))
            {
                sortResources.Remove(hit);
            }
            if (passthroughResources.Contains(hit))
            {
                passthroughResources.Remove(hit);
            }
        }
    }


    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        PushResources(passthroughResources, passthroughConvSettings, dt);
        PushResources(sortResources, sortConvSettings, dt);
    }
}
