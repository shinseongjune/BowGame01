using UnityEngine;

public class Item
{
    public int id;
    public string itemName;
    public string description;

    public Sprite icon;

    public int MAX_COUNT;

    public int? defaultSkill;
    public int? subSkill;

    public float dropRate;
    public int dropCount;
}