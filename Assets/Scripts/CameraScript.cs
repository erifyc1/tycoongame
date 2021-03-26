using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

	//[SerializeField]
	//private float zoom = 10f;
	private float vert;
	private float horiz;

	private float speed = 20f;

	Vector2 deltaPos;

	private Vector3 CameraRotation;

	private Vector2 scroll;
	[SerializeField] float camSpeed;
	[SerializeField] Vector3 startPos;
	[SerializeField] Vector3 startRot;


    public void OnVertical(InputAction.CallbackContext context)
    {
        
        vert = context.ReadValue<float>();
        
    }

	public void OnHorizontal(InputAction.CallbackContext context)
    {
        
        horiz = context.ReadValue<float>();
        
    }

	public void OnPanVert(InputAction.CallbackContext context)
	{

	}

	public void OnPanHoriz(InputAction.CallbackContext context)
	{

	}

	public void OnDeltaMouse(InputAction.CallbackContext context)
    {

        deltaPos = context.ReadValue<Vector2>();
    }
	

	public void OnScroll(InputAction.CallbackContext context)
    {
        
        scroll = context.ReadValue<Vector2>();
        
    }

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		//transform.position = startPos;
		//transform.rotation = Quaternion.Euler(startRot);
	}

	// Update is called once per frame
	void Update()
	{

		float RotationX = speed * deltaPos.x * Time.deltaTime;
		float RotationY = speed * deltaPos.y * Time.deltaTime;
		
		CameraRotation = Camera.main.transform.rotation.eulerAngles;
		
		CameraRotation.x -= RotationY;
		CameraRotation.y += RotationX;

		//Debug.Log(new Vector2(CameraRotation.x, CameraRotation.y));

		if (CameraRotation.x > 90 && CameraRotation.x < 270)
		{
			CameraRotation.x = CameraRotation.x > 180 ? 270 : 90;
		}
		
		Camera.main.transform.rotation = Quaternion.Euler(CameraRotation);


	}
	private void FixedUpdate()
	{
		transform.position += transform.forward*vert*camSpeed + transform.right*horiz*camSpeed;
	}

	void OnGUI()
	{
		// float zoomChange = scroll.y / (-100);
		// zoom = (zoom + zoomChange < 10f) ? 10f : zoom + zoomChange;
		// transform.position = new Vector3(transform.position.x, zoom, transform.position.z);
	}
}
