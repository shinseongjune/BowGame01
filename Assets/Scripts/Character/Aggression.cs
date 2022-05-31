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
    public Type type;
    public float damage;
    public GameObject attacker;
    public GameObject target;

    public Aggression(string name, Type type, float damage, GameObject attacker, GameObject target)
    {
        this.name = name;
        this.type = type;
        this.damage = damage;
        this.attacker = attacker;
        this.target = target;
    }
}
