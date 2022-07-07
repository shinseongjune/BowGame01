using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aggression
{
    public enum Type
    {
        Attack,
        Spell
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Lightning
    }

    public string name;
    public Type type;
    public Dictionary<DamageType, float> damages;
    public GameObject attacker;
    public GameObject target;

    public Aggression(string name, Type type, GameObject attacker, GameObject target)
    {
        this.name = name;
        this.type = type;
        this.attacker = attacker;
        this.target = target;
    }
}
