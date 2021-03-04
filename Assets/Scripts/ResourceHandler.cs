using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    [SerializeField]
    string[] resourceNames;

    [SerializeField]
    GameObject[] resourcePrefabs;
    private Dictionary<string, GameObject> resourceMap = new Dictionary<string, GameObject>();


    public void GenerateResource(Vector3 location, string type, int value)
    {
        if (resourceMap[type] != null)
        {
            GameObject r = Instantiate(resourceMap[type]);
            r.transform.position = location;
            r.GetComponent<ResourceItem>().SetValue(value);
        }
    }


    void Start()
    {
        for (int i = 0; i < resourcePrefabs.Length; i++)
        {
            resourceMap.Add(resourceNames[i], resourcePrefabs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
