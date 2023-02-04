using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    public bool enableEdgeScrolling = false;
    private float CAMERA_MIN_X = -50f;
    private float CAMERA_MIN_Y = -50f;
    private float CAMERA_MAX_X = 50f;
    private float CAMERA_MAX_Y = 50f;

    private float CAMERA_MIN_ZOOM = 5f;
    private float CAMERA_MAX_ZOOM = 20f;
    private Vector3 cameraFollowPosition;

    private bool isRightClickDragging = false;
    private Vector3 initialMousePosition = Vector3.zero;
    private Vector3 initialCameraPosition = Vector3.zero;

    private float rightClickDragSensitivityMultiplier = 0.5f;

    public void centerTheScreen() {
        cameraFollowPosition.x = 0;
        cameraFollowPosition.y = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow.Setup(() => {return cameraFollowPosition;});
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y == -1)
        {
            if (Camera.main.orthographicSize < CAMERA_MAX_ZOOM)
            {
                Camera.main.orthographicSize += 1.0f;
            }
        }

        if (Input.mouseScrollDelta.y == 1)
        {
            if (Camera.main.orthographicSize > CAMERA_MIN_ZOOM)
            {
                Camera.main.orthographicSize -= 1.0f;
            }
        }
        if (Input.GetMouseButtonDown(1) && !isRightClickDragging) {
            isRightClickDragging = true;
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialCameraPosition = transform.localPosition;
        }

        if (Input.GetMouseButtonUp(1)) {
            isRightClickDragging = false;
        }

        if (isRightClickDragging) {
            cameraFollowPosition = initialCameraPosition + (transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - transform.InverseTransformPoint(initialMousePosition)) * rightClickDragSensitivityMultiplier;
        } else {
            float moveAmount = 50f;
            if (Input.GetKey(KeyCode.W)) {
                cameraFollowPosition.y += moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S)) {
                cameraFollowPosition.y -= moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A)) {
                cameraFollowPosition.x -= moveAmount * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D)) {
                cameraFollowPosition.x += moveAmount * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Space)) {
                centerTheScreen();
            }

            if (enableEdgeScrolling) {
                float edgeSize = 10f;
                if (Input.mousePosition.x > Screen.width - edgeSize) {
                    cameraFollowPosition.x += moveAmount * Time.deltaTime;
                }

                if (Input.mousePosition.x < edgeSize) {
                    cameraFollowPosition.x -= moveAmount * Time.deltaTime;
                }

                if (Input.mousePosition.y > Screen.height - edgeSize) {
                    cameraFollowPosition.y += moveAmount * Time.deltaTime;
                }

                if (Input.mousePosition.y < edgeSize) {
                    cameraFollowPosition.y -= moveAmount * Time.deltaTime;
                }
            }
            
        }

        cameraFollowPosition.x = Mathf.Max(cameraFollowPosition.x, CAMERA_MIN_X);
        cameraFollowPosition.x = Mathf.Min(cameraFollowPosition.x, CAMERA_MAX_X);
        cameraFollowPosition.y = Mathf.Max(cameraFollowPosition.y, CAMERA_MIN_Y);
        cameraFollowPosition.y = Mathf.Min(cameraFollowPosition.y, CAMERA_MAX_Y);
    }
}
