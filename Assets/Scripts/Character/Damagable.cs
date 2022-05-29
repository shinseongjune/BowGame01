using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField]
    Stats stats;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    public void Damaged(float damage)
    {

    }
}
