using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    public enum Mode
    {
        Idle,
        Hunt,
        Alert,
        Combat,
        Defeated
    }

    public Vector3 originPosition;

    public GameObject target;
    public NavMeshAgent agent;

    public Mode mode = Mode.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.Idle:
                break;
            case Mode.Hunt:
                break;
            case Mode.Alert:
                break;
            case Mode.Combat:
                break;
            case Mode.Defeated:
                break;
        }  
    }
}
