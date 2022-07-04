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

    private void Start()
    {
        for (int i = 0; i < (int)Stat.Type.__COUNT; i++)
        {
            stats[i] = new Stat();
            stats[i].type = (Stat.Type)i;
            stats[i].isDirty = true;
            //TODO: 게임매니저에서 value 데이터 가져오기.
        }
        stats[(int)Stat.Type.MaxHP].name = "최대 체력";
        stats[(int)Stat.Type.MaxMP].name = "최대 마력";
        stats[(int)Stat.Type.Attack].name = "물리 공격력";
        stats[(int)Stat.Type.Fire].name = "화염 공격력";
        stats[(int)Stat.Type.Ice].name = "냉기 공격력";
        stats[(int)Stat.Type.Lightning].name = "번개 공격력";
        stats[(int)Stat.Type.Armor].name = "방어력";
        stats[(int)Stat.Type.FireResistance].name = "화염 저항";
        stats[(int)Stat.Type.IceResistance].name = "냉기 저항";
        stats[(int)Stat.Type.LightningResistance].name = "번개 저항";
        stats[(int)Stat.Type.ArmorPenetration].name = "방어 관통";
        stats[(int)Stat.Type.FirePenetration].name = "화염 관통";
        stats[(int)Stat.Type.IcePenetration].name = "냉기 관통";
        stats[(int)Stat.Type.LightningPenetration].name = "번개 관통";
        stats[(int)Stat.Type.MovementSpeed].name = "이동 속도";
        stats[(int)Stat.Type.CooldownReduction].name = "쿨타임 감소";
        stats[(int)Stat.Type.AttackCriticalChance].name = "공격 치명타 확률";
        stats[(int)Stat.Type.AttackCriticalDamage].name = "공격 치명타 피해";
        stats[(int)Stat.Type.SpellCriticalChance].name = "주문 치명타 확률";
        stats[(int)Stat.Type.SpellCriticalDamage].name = "주문 치명타 피해";

        stats[(int)Stat.Type.MaxHP].description = "체력 최대치입니다.";
        stats[(int)Stat.Type.MaxMP].description = "마력 최대치입니다.";
        stats[(int)Stat.Type.Attack].description = "물리 속성 공격을 할 때 적용되는 공격력입니다.";
        stats[(int)Stat.Type.Fire].description = "화염 속성 공격을 할 때 적용되는 공격력입니다.";
        stats[(int)Stat.Type.Ice].description = "냉기 속성 공격을 할 때 적용되는 공격력입니다.";
        stats[(int)Stat.Type.Lightning].description = "번개 속성 공격을 할 때 적용되는 공격력입니다.";
        stats[(int)Stat.Type.Armor].description = "물리 속성 피해를 감소시킵니다.";
        stats[(int)Stat.Type.FireResistance].description = "화염 속성 피해를 감소시킵니다.";
        stats[(int)Stat.Type.IceResistance].description = "냉기 속성 피해를 감소시킵니다.";
        stats[(int)Stat.Type.LightningResistance].description = "번개 속성 피해를 감소시킵니다.";
        stats[(int)Stat.Type.ArmorPenetration].description = "방어력을 무시합니다.";
        stats[(int)Stat.Type.FirePenetration].description = "화염 저항을 무시합니다.";
        stats[(int)Stat.Type.IcePenetration].description = "냉기 저항을 무시합니다.";
        stats[(int)Stat.Type.LightningPenetration].description = "번개 저항을 무시합니다";
        stats[(int)Stat.Type.MovementSpeed].description = "캐릭터가 이동하는 속도입니다.";
        stats[(int)Stat.Type.CooldownReduction].description = "기술 쿨타임을 감소시킵니다.";
        stats[(int)Stat.Type.AttackCriticalChance].description = "공격 시 치명타가 발생할 확률입니다.";
        stats[(int)Stat.Type.AttackCriticalDamage].description = "공격 치명타의 피해 배율입니다.";
        stats[(int)Stat.Type.SpellCriticalChance].description = "주문 시전 시 치명타가 발생할 확률입니다.";
        stats[(int)Stat.Type.SpellCriticalDamage].description = "주문 치명타의 피해 배율입니다.";
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
    }
}
