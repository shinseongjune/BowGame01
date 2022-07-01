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

    List<SpecialEffect> specialEffects = new();

    private void Start()
    {
        for (int i = 0; i < (int)Stat.Type.__COUNT; i++)
        {
            stats[i] = new Stat();
            stats[i].type = (Stat.Type)i;
            stats[i].isDirty = true;
            //TODO: 게임매니저에서 value 데이터 가져오기.
        }
        //Debug: 테스트용 코드
        stats[(int)Stat.Type.MaxHP].baseValue = 100;
        hp = stats[(int)Stat.Type.MaxHP].Value;
        mp = stats[(int)Stat.Type.MaxMP].Value;
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Die();
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
            foreach(StatModifier mod in stat.modifiers)
            {
                if (mod.source == source)
                {
                    RemoveStatModifier(stat.type, mod);
                }
            }
        }
    }

    public void Die()
    {
        //TODO: 죽음 애니메이션. 사라질 시간 조절하기. 시체를 남겨야할수도.
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
    }

    public void RemoveSpecialEffect(SpecialEffect effect)
    {
        foreach (Stat stat in stats)
        {
            foreach (StatModifier mod in stat.modifiers)
            {
                if (mod.source == effect)
                {
                    stat.modifiers.Remove(mod);
                }
            }
        }

        specialEffects.Remove(effect);
    }

    public void RemoveSpecialEffectFromSource(object source)
    {
        foreach (SpecialEffect effect in specialEffects)
        {
            if (effect.source == source)
            {
                RemoveSpecialEffect(effect);
            }
        }
    }
}
