using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float vDir = Input.GetAxis("Vertical") * Time.deltaTime * 10;
        float hDir = Input.GetAxis("Horizontal") * Time.deltaTime * 10;
        transform.position += new Vector3(hDir, 0, vDir);
    }
}
