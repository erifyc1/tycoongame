using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    [Header("Resource Types\n")]
        [SerializeField]
        string[] resourceNames;

        [SerializeField]
        GameObject[] resourcePrefabs;

    [Header("Upgrade Types\n")]
        [SerializeField]
        string[] upgradeNames;


    private Dictionary<string, GameObject> resourceMap = new Dictionary<string, GameObject>();

    private Dictionary<string, bool> blankUpgradeTemplate = new Dictionary<string, bool>();

    private GameManager gameManager;

    public GameObject SwapResource(GameObject current, string newType, int newValue) // swaps current with a new resource with newType and newValue
    {
        Vector3 loc = current.transform.position + Vector3.up;
        Dictionary<string, bool> upgrades = current.GetComponent<ResourceItem>().GetUpgrades();
        Destroy(current);

        GameObject newResource = GenerateResource(loc, newType, newValue);
        newResource.GetComponent<ResourceItem>().SetUpgrades(upgrades);
        return newResource;
    }


    public GameObject GenerateResource(Vector3 location, string type, int value)
    {
        if (resourceMap[type] != null)
        {
            GameObject r = Instantiate(resourceMap[type]);
            r.transform.position = location;
            r.GetComponent<ResourceItem>().SetValue(value);
            r.GetComponent<ResourceItem>().SetUpgrades(blankUpgradeTemplate);
            return r;
        }
        return null;
    }

    public void SellResource(GameObject res)
    {
        if (TryGetComponent(typeof(ResourceItem), out Component c))
        {
            int val = c.GetComponent<ResourceItem>().GetValue();
            Destroy(res);
            gameManager.AddScore(val);

        }
        else Debug.Log("cannot sell non resource");
    }


    void Start()
    {
        for (int i = 0; i < resourcePrefabs.Length; i++)
        {
            resourceMap.Add(resourceNames[i], resourcePrefabs[i]);
        }

        foreach (string s in upgradeNames)
        {
            blankUpgradeTemplate.Add(s, false);
        }

        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
