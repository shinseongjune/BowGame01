using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    const float DISTANCE_TO_RIG = 13.0f;

    float mouseMovementX = 0f;
    float mouseMovementY = 0f;

    const float MOUSE_MOVEMENT_SPEED = 2.7f;

    const float MOUSE_MOVEMENT_Y_MIN = 10.0f;
    const float MOUSE_MOVEMENT_Y_MAX = 80.0f;

    [SerializeField]
    Transform cameraRig;

    void Start()
    {
        transform.LookAt(cameraRig);
        mouseMovementX = transform.eulerAngles.y;
        mouseMovementY = transform.eulerAngles.x;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mouseMovementX += Input.GetAxis("Mouse X") * MOUSE_MOVEMENT_SPEED;
            mouseMovementY -= Input.GetAxis("Mouse Y") * MOUSE_MOVEMENT_SPEED;

            mouseMovementY = Mathf.Clamp(mouseMovementY, MOUSE_MOVEMENT_Y_MIN, MOUSE_MOVEMENT_Y_MAX);
        }

        Quaternion rotation = Quaternion.Euler(mouseMovementY, mouseMovementX, 0);

        Vector3 distanceVector3 = new(0, 0, -DISTANCE_TO_RIG);

        transform.position = rotation * distanceVector3 + cameraRig.position;
        transform.rotation = rotation;
    }
}
