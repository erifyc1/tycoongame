using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField]
	private float zoom = 10f;

	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(0, 10, 0);
	}

	// Update is called once per frame
	void Update()
	{
		float vDir = Input.GetAxis("Vertical") * Time.deltaTime * 50 * Mathf.Sqrt(zoom / 10);
		float hDir = Input.GetAxis("Horizontal") * Time.deltaTime * 50 * Mathf.Sqrt(zoom / 10);
		transform.position += new Vector3(hDir, 0, vDir);
	}

	void OnGUI()
	{
		float zoomChange = Input.mouseScrollDelta.y * -5;
		zoom = (zoom + zoomChange < 0) ? 0 : zoom + zoomChange;
		transform.position = new Vector3(transform.position.x, zoom, transform.position.z);
	}
}
