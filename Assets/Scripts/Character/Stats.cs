using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public enum Type
    {
        MaxHP,
        MaxMP,
        Attack,
        Fire,
        Ice,
        Lightning,
        Armor,
        FireResistance,
        IceResistance,
        LightningResistance,
        ArmorPenetration,
        FirePenetration,
        IcePenetration,
        LightningPenetration,
        MovementSpeed,
        CooldownReduction,
        AttackCriticalChance,
        AttackCriticalDamage,
        SpellCriticalChance,
        SpellCriticalDamage,
        __COUNT,
    }

    public string name;
    public string description;

    public float baseValue;
    public bool isDirty;
    float totalValue;

    public Type type;

    public readonly List<StatModifier> modifiers = new List<StatModifier>();

    public float Value
    {
        get
        {
            if (isDirty)
            {
                totalValue = baseValue;
                float groupA = 1;
                float groupB = 1;
                float groupC = 1;
                float groupD = 1;

                foreach (StatModifier modifier in modifiers)
                {
                    switch (modifier.type)
                    {
                        case StatModifier.Type.BaseFlat:
                            totalValue += modifier.value;
                            break;
                        case StatModifier.Type.BaseMult:
                            totalValue *= modifier.value;
                            break;
                        case StatModifier.Type.TotalFlat:
                            totalValue += modifier.value;
                            break;
                        case StatModifier.Type.TotalMultA:
                            groupA += modifier.value;
                            break;
                        case StatModifier.Type.TotalMultB:
                            groupB += modifier.value;
                            break;
                        case StatModifier.Type.TotalMultC:
                            groupC += modifier.value;
                            break;
                        case StatModifier.Type.TotalMultD:
                            groupD += modifier.value;
                            break;
                    }
                }
                totalValue *= groupA * groupB * groupC * groupD;

                isDirty = false;
                return totalValue;
            }
            return totalValue;
        }
    }
}

public class Stats : MonoBehaviour
{
    public int characterId = 0;

    public float hp;
    public float mp;

    public Stat[] stats = new Stat[(int)Stat.Type.__COUNT];

    public List<SpecialEffect> specialEffects = new();

    public bool isSpecialEffectsUpdated = false;

    private void Start()
    {
        for (int i = 0; i < (int)Stat.Type.__COUNT; i++)
        {
            stats[i] = new Stat();
            stats[i].type = (Stat.Type)i;
            stats[i].isDirty = true;
            //TODO: ?????????????? value ?????? ????????.
        }
        stats[(int)Stat.Type.MaxHP].name = "???? ????";
        stats[(int)Stat.Type.MaxMP].name = "???? ????";
        stats[(int)Stat.Type.Attack].name = "???? ??????";
        stats[(int)Stat.Type.Fire].name = "???? ??????";
        stats[(int)Stat.Type.Ice].name = "???? ??????";
        stats[(int)Stat.Type.Lightning].name = "???? ??????";
        stats[(int)Stat.Type.Armor].name = "??????";
        stats[(int)Stat.Type.FireResistance].name = "???? ????";
        stats[(int)Stat.Type.IceResistance].name = "???? ????";
        stats[(int)Stat.Type.LightningResistance].name = "???? ????";
        stats[(int)Stat.Type.ArmorPenetration].name = "???? ????";
        stats[(int)Stat.Type.FirePenetration].name = "???? ????";
        stats[(int)Stat.Type.IcePenetration].name = "???? ????";
        stats[(int)Stat.Type.LightningPenetration].name = "???? ????";
        stats[(int)Stat.Type.MovementSpeed].name = "???? ????";
        stats[(int)Stat.Type.CooldownReduction].name = "?????? ????";
        stats[(int)Stat.Type.AttackCriticalChance].name = "???? ?????? ????";
        stats[(int)Stat.Type.AttackCriticalDamage].name = "???? ?????? ????";
        stats[(int)Stat.Type.SpellCriticalChance].name = "???? ?????? ????";
        stats[(int)Stat.Type.SpellCriticalDamage].name = "???? ?????? ????";

        stats[(int)Stat.Type.MaxHP].description = "???? ????????????.";
        stats[(int)Stat.Type.MaxMP].description = "???? ????????????.";
        stats[(int)Stat.Type.Attack].description = "???? ???? ?????? ?? ?? ???????? ????????????.";
        stats[(int)Stat.Type.Fire].description = "???? ???? ?????? ?? ?? ???????? ????????????.";
        stats[(int)Stat.Type.Ice].description = "???? ???? ?????? ?? ?? ???????? ????????????.";
        stats[(int)Stat.Type.Lightning].description = "???? ???? ?????? ?? ?? ???????? ????????????.";
        stats[(int)Stat.Type.Armor].description = "???? ???? ?????? ????????????.";
        stats[(int)Stat.Type.FireResistance].description = "???? ???? ?????? ????????????.";
        stats[(int)Stat.Type.IceResistance].description = "???? ???? ?????? ????????????.";
        stats[(int)Stat.Type.LightningResistance].description = "???? ???? ?????? ????????????.";
        stats[(int)Stat.Type.ArmorPenetration].description = "???????? ??????????.";
        stats[(int)Stat.Type.FirePenetration].description = "???? ?????? ??????????.";
        stats[(int)Stat.Type.IcePenetration].description = "???? ?????? ??????????.";
        stats[(int)Stat.Type.LightningPenetration].description = "???? ?????? ??????????";
        stats[(int)Stat.Type.MovementSpeed].description = "???????? ???????? ??????????.";
        stats[(int)Stat.Type.CooldownReduction].description = "???? ???????? ????????????.";
        stats[(int)Stat.Type.AttackCriticalChance].description = "???? ?? ???????? ?????? ??????????.";
        stats[(int)Stat.Type.AttackCriticalDamage].description = "???? ???????? ???? ??????????.";
        stats[(int)Stat.Type.SpellCriticalChance].description = "???? ???? ?? ???????? ?????? ??????????.";
        stats[(int)Stat.Type.SpellCriticalDamage].description = "???? ???????? ???? ??????????.";
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Die();
        }

        for (int i = specialEffects.Count - 1; i >= 0; i--)
        {
            SpecialEffect effect = specialEffects[i];
            if (effect.restTime == null)
            {
                continue;
            }
            else
            {
                effect.restTime -= Time.deltaTime;

                if (effect.restTime <= 0)
                {
                    RemoveSpecialEffect(effect);
                }
            }
        }
    }

    public void AddStatModifier(Stat.Type type, StatModifier mod)
    {
        float beforeValue = 0;
        if (type == Stat.Type.MaxHP || type == Stat.Type.MaxMP)
        {
            beforeValue = stats[(int)type].Value;
        }
        stats[(int)type].modifiers.Add(mod);
        stats[(int)type].isDirty = true;
        if (type == Stat.Type.MaxHP)
        {
            if (hp > stats[(int)Stat.Type.MaxHP].Value)
            {
                hp = stats[(int)Stat.Type.MaxHP].Value;
            }
            else
            {
                hp += stats[(int)type].Value - beforeValue;
            }
        }
        else if (type == Stat.Type.MaxMP)
        {
            if (mp > stats[(int)Stat.Type.MaxMP].Value)
            {
                mp = stats[(int)Stat.Type.MaxMP].Value;
            }
            else
            {
                mp += stats[(int)type].Value - beforeValue;
            }
        }
    }

    public void RemoveStatModifier(Stat.Type type, StatModifier mod)
    {
        float beforeValue = 0;
        if (type == Stat.Type.MaxHP || type == Stat.Type.MaxMP)
        {
            beforeValue = stats[(int)type].Value;
        }
        stats[(int)type].modifiers.Remove(mod);
        stats[(int)type].isDirty = true;
        if (type == Stat.Type.MaxHP)
        {
            if (hp > stats[(int)type].Value)
            {
                hp = stats[(int)type].Value;
            }
            else
            {
                hp += stats[(int)type].Value - beforeValue;
            }
        }
        else if (type == Stat.Type.MaxMP)
        {
            if (mp > stats[(int)type].Value)
            {
                mp = stats[(int)type].Value;
            }
            else
            {
                mp += stats[(int)type].Value - beforeValue;
            }
        }
    }

    public void RemoveStatModifierFromSource(object source)
    {
        foreach(Stat stat in stats)
        {
            for (int i = stat.modifiers.Count - 1; i >= 0; i--)
            {
                StatModifier mod = stat.modifiers[i];
                if (mod.source == source)
                {
                    RemoveStatModifier(stat.type, mod);
                }
            }
        }
    }

    public void Die()
    {
        //TODO: ???? ??????????. ?????? ???? ????????. ?????? ????????????.
        ItemDroppable droppable = GetComponent<ItemDroppable>();
        if (droppable != null)
        {
            droppable.DropItem(characterId);
        }

        Destroy(gameObject);
    }

    public void AddSpecialEffect(SpecialEffect effect)
    {
        if (effect.isUnique)
        {
            foreach (SpecialEffect e in specialEffects)
            {
                if (e.name == effect.name)
                {
                    return;
                }
            }
        }

        specialEffects.Add(effect);
        
        foreach (var mods in effect.modifiers)
        {
            Stat.Type type = mods.Key;
            stats[(int)type].modifiers.Add(mods.Value);
        }

        isSpecialEffectsUpdated = true;
    }

    public void RemoveSpecialEffect(SpecialEffect effect)
    {
        for (int i = 0; i < (int)Stat.Type.__COUNT; i++)
        {
            Stat stat = stats[i];
            for (int j = stat.modifiers.Count - 1; j >= 0; j--)
            {
                StatModifier mod = stat.modifiers[j];
                if (mod.source == effect)
                {
                    stat.modifiers.Remove(mod);
                }
            }
        }

        specialEffects.Remove(effect);

        isSpecialEffectsUpdated = true;
    }

    public void RemoveSpecialEffectFromSource(object source)
    {
        for (int i = specialEffects.Count - 1; i >= 0; i--)
        {
            SpecialEffect effect = specialEffects[i];
            if (effect.source == source)
            {
                RemoveSpecialEffect(effect);
            }
        }

        isSpecialEffectsUpdated = true;
    }
}
