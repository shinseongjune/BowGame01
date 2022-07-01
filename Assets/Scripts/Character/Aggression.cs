using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aggression
{
    public enum Type
    {
        Attack,
        Fire,
        Ice,
        Lightning
    }

    public string name;
    public Dictionary<Type, float> damages;
    public GameObject attacker;
    public GameObject target;

    public Aggression(string name, GameObject attacker, GameObject target)
    {
        this.name = name;
        this.attacker = attacker;
        this.target = target;
    }
}
