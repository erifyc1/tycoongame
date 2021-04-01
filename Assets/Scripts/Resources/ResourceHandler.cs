using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] List<ResourceType> resourceTypes;

    private List<QueuedResource> queuedResources = new List<QueuedResource>();
    private float queueSpawnTime = 0.8f;
    private float queueTimer = 0f;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

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



    public void QueueResources(int locId, Vector3 location, Vector3 velocity, Vector3 angVelocity, ResourceType[] resouces) // needs array input due to formatting of recipes
    {
        foreach (ResourceType resource in resouces)
        {
            QueueResource(locId, location, velocity, angVelocity, resource);
        }
    }

    public void QueueResource(int locId, Vector3 location, Vector3 velocity, Vector3 angVelocity, ResourceType resouce)
    {
        QueuedResource existingQueued = queuedResources.Find((qr) => qr.id == locId);

        if (existingQueued == null)
        {
            List<ResourceType> newQueue = new List<ResourceType>();
            newQueue.Add(resouce);

            queuedResources.Add(new QueuedResource(locId, location, velocity, angVelocity, newQueue));
        }
        else
        {
            existingQueued.resources.Add(resouce);
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

    public class QueuedResource
    {
        public int id;
        public Vector3 location;
        public Vector3 velocity;
        public Vector3 angVelocity;
        public List<ResourceType> resources;


        public QueuedResource(int id, Vector3 location, Vector3 velocity, Vector3 angVelocity, List<ResourceType> resouces)
        {
            this.id = id;
            this.location = location;
            this.velocity = velocity;
            this.angVelocity = angVelocity;
            this.resources = resouces;
        }


    }
    



    // Update is called once per frame
    void Update() 
    {
        queueTimer += Time.deltaTime;

        if (queueTimer >= queueSpawnTime && queuedResources.Count > 0)
        {
            queueTimer = 0;

            foreach (QueuedResource q in queuedResources)
            {
                if (q.resources.Count == 0) break;

                GenerateResource(q.location, q.velocity, q.angVelocity, q.resources[0]);
                q.resources.RemoveAt(0);

            }
        }
    }
}
