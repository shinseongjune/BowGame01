using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 frontDirection = Vector3.forward;
    Vector3 rightDirection = Vector3.right;

    Rigidbody body;

    public float speed = 8.0f;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetCameraDirections();

        Vector3 velocity = new();
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            velocity += rightDirection * horizontal * speed;
        }

        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical != 0)
        {
            velocity += frontDirection * vertical * speed;
        }

        body.velocity = velocity;
    }

    void GetCameraDirections()
    {
        frontDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * Vector3.forward;
        rightDirection = Quaternion.AngleAxis(90, Vector3.up) * frontDirection;
    }
}
