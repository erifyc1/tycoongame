using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] List<ResourceType> resourceTypes;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }
/*    public GameObject SwapResource(GameObject current, string newTypeName)
    {
        Vector3 loc = current.transform.position;// + Vector3.up;
        Destroy(current);

        GameObject newResource = GenerateResource(loc, newTypeName);
        return newResource;
    }

    public GameObject SwapResource(GameObject current, ResourceType newType) // swaps current with a new resource with newType and newValue
    {
        Vector3 loc = current.transform.position;// + Vector3.up;
        Vector3 velocity = current.GetComponent<Rigidbody>().velocity;
        Vector3 av = current.GetComponent<Rigidbody>().angularVelocity;
        Destroy(current);

        GameObject newResource = GenerateResource(loc, newType);
        newResource.GetComponent<Rigidbody>().velocity = velocity;
        newResource.GetComponent<Rigidbody>().angularVelocity = av;
        return newResource;
    }*/

/*    public GameObject GenerateResource(Vector3 location, string typeName) 
    {
        ResourceType type = resourceTypes.Find((t) => t.GetName() == typeName);
        return GenerateResource(location, type);
    }*/

    public GameObject GenerateResource(Vector3 location, ResourceType type)
    {
        return GenerateResource(location, Vector3.zero, Vector3.zero, type);
    }

    public GameObject GenerateResource(Vector3 location, Vector3 velocity, Vector3 angVector, ResourceType type)
    {
        if (type != null)
        {
            GameObject r = Instantiate(type.GetModel());
            r.transform.position = location;
            r.GetComponent<Rigidbody>().velocity = velocity;
            r.GetComponent<Rigidbody>().angularVelocity = angVector;
            r.GetComponent<ResourceItem>().SetResourceType(type);
            StartCoroutine(r.GetComponent<ResourceItem>().ActivateTransformable());
            r.transform.parent = transform;

            return r;
        }
        else return null;
    }



    public IEnumerator GenerateResources(Vector3 location, Vector3 velocity, Vector3 angVelocity, ResourceType[] resouces)
    {
        foreach (ResourceType resource in resouces)
        {
            GenerateResource(location, velocity, angVelocity, resource);
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void SellResource(GameObject res)
    {
        if (TryGetComponent(typeof(ResourceItem), out Component c))
        {
            int val = c.GetComponent<ResourceItem>().GetResourceType().GetValue();
            Destroy(res);
            gameManager.AddScore(val);

        }
        else Debug.Log("cannot sell non resource");
    }


    // Update is called once per frame
    void Update() { }
}
