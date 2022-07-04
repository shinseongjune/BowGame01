using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect
{
    public string name;
    public string description;

    public bool isDisplayed;
    public bool isUnique;

    public float? restTime;

    public Dictionary<Stat.Type, StatModifier> modifiers = new();
    
    public Sprite icon;

    public object source;

    public SpecialEffect(string name, string description, bool isDisplayed, bool isUnique, float? restTime, Sprite icon, object source)
    {
        this.name = name;
        this.description = description;
        this.isDisplayed = isDisplayed;
        this.isUnique = isUnique;
        this.restTime = restTime;
        this.icon = icon;
        this.source = source;
    }

    public virtual void Invoke()
    {

    }
}
