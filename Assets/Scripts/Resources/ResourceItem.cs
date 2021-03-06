using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    Rigidbody rb;
    ResourceHandler rh;

    private ResourceType resourceType;
    bool transformable = false;

    public void SetResourceType(ResourceType r)
    {
        resourceType = r;
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rh = GameObject.FindGameObjectWithTag("resourceHandler").GetComponent<ResourceHandler>();
    }


    public IEnumerator ActivateTransformable()
    {
        yield return new WaitForSeconds(0.1f);
        transformable = true;
    }

/*    void OnCollisionStay(Collision other)
    {
        string tag = other.gameObject.tag;
        if (tag == "conveyor")
        {
            Vector2 accel = other.gameObject.GetComponents<ConveyorScript>().GetAcceleration(other.GetContact(0).point) * Time.deltaTime;

            rb.velocity /= 1.01f;
            rb.velocity += new Vector3(accel.x / (rb.velocity.sqrMagnitude + 5), 0, accel.y / (rb.velocity.sqrMagnitude + 5));
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (transformable)
        {
            ResourceManipulator transformer = other.gameObject.GetComponent<ResourceManipulator>();

            if (transformer != null)
            {
                Vector3 loc = transform.position;
                Vector3 velocity = GetComponent<Rigidbody>().velocity;
                Vector3 av = GetComponent<Rigidbody>().angularVelocity;
                ResourceType[] outputResources = transformer.InputResource(gameObject);

                if (outputResources != null)
                {
                    rh.QueueResources(other.gameObject.GetInstanceID(), loc, velocity, av, outputResources);
                }
            }
        }
    }

}
