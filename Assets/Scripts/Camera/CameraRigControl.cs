using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigControl : MonoBehaviour
{
    [SerializeField]
    Transform player;

    public const float OFFSET_Y = 1.0f;

    void Start()
    {
        transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
    }

    void Update()
    {
        transform.position = player.position + new Vector3(0, OFFSET_Y, 0);
    }
}
