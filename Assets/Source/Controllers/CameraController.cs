/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// CameraController.cs
/// Class used to handle the motion of the camera throughout the scene

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    /// Variables

    public float panSpeed;
    public float zoomSpeed;

    /// Constructors
    

    /// Methods

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Move camera
        Vector3 newPosition = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) {
            newPosition -= new Vector3(panSpeed, 0f, 0f);         // LEFT
        }
        if (Input.GetKey(KeyCode.D)) {
            newPosition += new Vector3(panSpeed, 0f, 0f);         // RIGHT
        }
        if (Input.GetKey(KeyCode.W)) {
            newPosition += new Vector3(0f, 0f, panSpeed);         // UP
        }
        if (Input.GetKey(KeyCode.S)) {
            newPosition -= new Vector3(0f, 0f, panSpeed);         // DOWN
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            newPosition *= 2;
        }
        newPosition.y = -(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);

        Camera.main.transform.position += (newPosition * Time.deltaTime);
	
	}
}
