using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigControl : MonoBehaviour
{
    [SerializeField]
    Transform player;

    public PlayerState state;

    public const float OFFSET_Y = 1.0f;
    public float SMOOTH_FACTOR = 0.05f;

    public float timer = 0f;

    bool isSkillMoved = false;

    void Start()
    {
        state = player.GetComponent<PlayerState>();
        transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
    }

    void LateUpdate()
    {
        if (state.isSkillMoving)
        {
            isSkillMoved = true;
        }

        if (isSkillMoved)
        {
            transform.position = Vector3.Slerp(transform.position, player.position + new Vector3(0, OFFSET_Y, 0), SMOOTH_FACTOR);
            SMOOTH_FACTOR += 0.1f + SMOOTH_FACTOR * 2f * Time.deltaTime;
            timer += Time.deltaTime;
            if (Vector3.Distance(transform.position, player.position + new Vector3(0, 1, 0)) < 0.2f || timer >= 0.7f) {
                isSkillMoved = false;
                transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
                SMOOTH_FACTOR = 0.05f;
                timer = 0f;
            }
        }
        else
        {
            transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
        }

    }
}
