
using System.Collections.Generic;
using UnityEngine;


public class ResourcePusher : MonoBehaviour
{
    public void PushResources(List<GameObject> resources, ConveyorSettings conveyorSettings, float dt)
    {
        CommonProperties common;
        if (transform.parent.TryGetComponent(typeof(CommonProperties), out Component comp))
        {
            common = comp.GetComponent<CommonProperties>();
        }
        else
        {
            throw new MissingComponentException("missing common properties from parent object");
        }


        for (int i = 0; i < resources.Count; i++)
        {
            GameObject resource = resources[i];
            if (resource == null)
            {
                resources.Remove(resource);
                i--;
            }
            else
            {
                Rigidbody rb = resource.GetComponent<Rigidbody>();
                Vector3 difference = resource.transform.position - transform.position;
                Vector3 adjNormDiff = Utils.AdjustRelativePos(difference.normalized, common.GetFacing(), false);
                Vector3 push = Utils.AdjustRelativePos(conveyorSettings.CalculateAccel(rb.velocity.magnitude, dt) * conveyorSettings.CalculatePush(adjNormDiff), common.GetFacing(), true);
                
                rb.velocity += push;
            }
        }
    }
}
