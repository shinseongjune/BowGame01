using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigControl : MonoBehaviour
{
    [SerializeField]
    Transform player;

    public PlayerState state;

    public const float OFFSET_Y = 1.0f;

    void Start()
    {
        state = player.GetComponent<PlayerState>();
        transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
    }

    void Update()
    {
        if (state.isBuilding && state.buildingFloor == 1)
        {
            transform.position = player.position + new Vector3(0, OFFSET_Y + 4.4f, 0);
        }
        else
        {
            transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
        }
    }
}
