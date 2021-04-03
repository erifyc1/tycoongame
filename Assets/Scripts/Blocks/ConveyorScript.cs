using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : ResourcePusher
{
    [SerializeField] ConveyorSettings convSettings;

    private List<GameObject> resourcesOnTop = new List<GameObject>();
    private CommonProperties common;


    void Start()
    {
        common = transform.parent.GetComponent<CommonProperties>();
        if (convSettings == null) throw new MissingReferenceException("missing conveyor settings");
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            //GameObject resource = transform.parent.gameObject;
            resourcesOnTop.Add(hit);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "resource")
        {
            //GameObject resource = transform.parent.gameObject;
            if (resourcesOnTop.Contains(hit))
            {
                resourcesOnTop.Remove(hit);
            } 
        }
    }


    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        PushResources(resourcesOnTop, convSettings, dt);
    }
}
