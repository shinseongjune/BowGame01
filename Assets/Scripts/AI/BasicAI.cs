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
        Combat,
        Defeated
    }

    public GameObject target;
    public NavMeshAgent agent;

    public Mode mode;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mode = Mode.Idle;
    }

    void Update()
    {
        
    }
}
