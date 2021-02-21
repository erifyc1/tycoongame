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
            Vector2 accel = other.gameObject.GetComponents<IConveyor>()[0].getAcceleration(other.GetContact(0).point) * Time.deltaTime;
            rb.velocity += new Vector3(accel.x, 0, accel.y)*(1/(rb.velocity.magnitude + 5));
        }
    }
}
