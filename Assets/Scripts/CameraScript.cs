using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField]
	private float zoom = 10f;
	private float vert;
	private float horiz;
	private Vector2 scroll;

    public void OnVertical(InputAction.CallbackContext context)
    {
        
        vert = context.ReadValue<float>();
        
    }

	public void OnHorizontal(InputAction.CallbackContext context)
    {
        
        horiz = context.ReadValue<float>();
        
    }

		public void OnScroll(InputAction.CallbackContext context)
    {
        
        scroll = context.ReadValue<Vector2>();
        
    }
	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(0, 10, 0);
	}

	// Update is called once per frame
	void Update()
	{
		float vDir = vert * Time.deltaTime * 50 * Mathf.Sqrt(zoom / 10);
		float hDir = horiz * Time.deltaTime * 50 * Mathf.Sqrt(zoom / 10);
		transform.position += new Vector3(hDir, 0, vDir);
	}

	void OnGUI()
	{
		float zoomChange = scroll.y / (-100);
		zoom = (zoom + zoomChange < 10f) ? 10f : zoom + zoomChange;
		transform.position = new Vector3(transform.position.x, zoom, transform.position.z);
	}
}
