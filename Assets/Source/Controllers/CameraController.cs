/// RTS-Project-01 -- Created by D. Sinclair, 2016
/// ================
/// CameraController.cs
/// Class used to handle the motion of the camera throughout the scene

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    /// Variables

    public GameObject cameraParent;

    public float keyPanSensitivity;
    public float mousePanSensitivity;
    public float zoomSensitivity;
    public float mouseRotationSensitivity;
    public float keyRotationSensitivity;

    public float cameraAngle;

    public const float MAX_ZOOM = 100;
    public const float MIN_ZOOM = 0;

    private Vector3 currentMousePosition;
    private Vector3 lastMousePosition;

    /// Methods

	// Use this for initialization
	void Start () {
        Camera.main.transform.Rotate(cameraAngle, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        currentMousePosition = Input.mousePosition;

        Pan();
        Rotate();
        Zoom();

        lastMousePosition = currentMousePosition;
	}

    private void Pan() {
        // Pan camera via mouse
        if (Input.GetMouseButtonDown(2)) {
            // Set last mouse position to current mouse position for initial click
            lastMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButton(2)) {
            // Calculate difference between current and last mouse position before translate and then update last mouse position to current
            Vector3 mousePositionDelta = currentMousePosition - lastMousePosition;

            cameraParent.transform.Translate(mousePositionDelta.x * -1 * mousePanSensitivity, 0, mousePositionDelta.y * -1 * mousePanSensitivity);
        }

        // Pan camera via keyboard
        Vector2 controlDelta = Vector2.zero;

        if (Input.GetKey(KeyCode.A)) {
            controlDelta.x -= 1;         // LEFT
        }
        if (Input.GetKey(KeyCode.D)) {
            controlDelta.x += 1;         // RIGHT
        }
        if (Input.GetKey(KeyCode.W)) {
            controlDelta.y += 1;         // UP
        }
        if (Input.GetKey(KeyCode.S)) {
            controlDelta.y -= 1;         // DOWN
        }

        Vector3 movementDelta = new Vector3(controlDelta.x * keyPanSensitivity, 0, controlDelta.y * keyPanSensitivity);

        cameraParent.transform.Translate(movementDelta);
    }

    private void Zoom() {
        // Zoom the camera in or out
        float scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");

        Camera.main.transform.Translate(Vector3.forward * scrollWheelDelta * zoomSensitivity);

        // TODO: Figure out way to implement a working minimum and maximum zoom level
    }

    private void Rotate() {
        // Rotate camera via mouse
        if (Input.GetMouseButtonDown(1)) {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) {
            float mouseDelta = currentMousePosition.x - lastMousePosition.x;
            //Camera.main.transform.LookAt(WorldController.Instance.)
            cameraParent.transform.Rotate(0, mouseDelta * mouseRotationSensitivity, 0);
        }

        // Rotate camera via keyboard
        float keyDelta = 0;

        if (Input.GetKey(KeyCode.Q)) {
            keyDelta--;
        }
        if (Input.GetKey(KeyCode.E)) {
            keyDelta++;
        }

        cameraParent.transform.Rotate(0, keyDelta * keyRotationSensitivity, 0);
    }
}
