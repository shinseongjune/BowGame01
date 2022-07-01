using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect
{
    public string name;
    public string description;

    public bool isDisplayed = false;
    public bool isUnique = true;

    public float? restTime;

    public Dictionary<Stat.Type, StatModifier> modifiers = new();

    public object source;

    public virtual void Invoke()
    {

    }
}
