using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IActivatable
{
    [SerializeField]
    float spawnTime = 5f;
    private float timer = 0;
    private bool producing = false;
    private ResourceHandler res;
    [SerializeField] ResourceType spawnType;
	
    void Start()
    {
        res = GameObject.FindGameObjectWithTag("resourceHandler").GetComponent<ResourceHandler>();
    }
    public void Activate()
    {
        producing = true;

    }

    public void Deactivate()
    {
        producing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (producing)
        {
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                timer = 0;
                
                res.GenerateResource(transform.position + Vector3.up, spawnType);
                
            }
        }

    }
}
