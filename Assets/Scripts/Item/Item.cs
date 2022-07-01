using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string itemName;
    public string description;

    public Sprite icon;

    public int MAX_COUNT;

    public int? defaultSkill;

    public float dropRate;
    public int dropCount;
}

public class Equipment : Item
{
    public enum Type
    {
        Head,
        Shoulder,
        Hand,
        Leg,
        Feet,
    }

    public Type type;
    public Dictionary<Stat.Type, float> stats = new();

    public List<SpecialEffect> effects = new();
}