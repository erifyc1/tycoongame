using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCube : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
	
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "conveyor")
        {
            rb.velocity += other.transform.up * Time.deltaTime * other.gameObject.GetComponent<ConveyorScript>().conveyorSpeed;
        }
    }
}
