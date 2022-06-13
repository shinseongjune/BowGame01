using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerState state;

    Vector3 frontDirection = Vector3.forward;
    Vector3 rightDirection = Vector3.right;

    Rigidbody body;

    public float speed = 8.0f;

    void Start()
    {
        state = GetComponent<PlayerState>();
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (state.isSkillMoving)
        {
            state.skillMovingTime -= Time.deltaTime;
            if (state.skillMovingTime <= 0)
            {
                state.isSkillMoving = false;
                body.constraints = RigidbodyConstraints.FreezeRotation;
                //TODO: 테스트 코드. 지울것
                GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                //테스트 코드 끝
            }
            return;
        }
        else
        {
            GetCameraDirections();

            Vector3 velocity = new();
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (state.isMovable)
            {
                if (horizontal != 0)
                {
                    velocity += speed * horizontal * rightDirection;
                }

                float vertical = Input.GetAxisRaw("Vertical");
                if (vertical != 0)
                {
                    velocity += speed * vertical * frontDirection;
                }
            }

            body.velocity = new(velocity.x, body.velocity.y, velocity.z);
        }
    }

    void GetCameraDirections()
    {
        frontDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * Vector3.forward;
        rightDirection = Quaternion.AngleAxis(90, Vector3.up) * frontDirection;
    }
}
