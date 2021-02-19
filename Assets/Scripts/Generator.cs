using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IActivatable
{
    [SerializeField]
    GameObject resourceCube;
    [SerializeField]
    float spawnTime = 5f;
    private float timer = 0;
    private bool producing = false;
	
    void Start()
    {

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
                if (resourceCube != null)
                {
                    Instantiate(resourceCube, new Vector3(transform.position.x, transform.position.y + 6, transform.position.z), new Quaternion(0, 0, 0, 0));
                }
            }
        }

    }
}
