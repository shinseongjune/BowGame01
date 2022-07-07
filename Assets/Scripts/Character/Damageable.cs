using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Stats target;

    private void Start()
    {
        target = GetComponent<Stats>();
    }

    public void Damaged(Aggression aggression, Dictionary<Aggression.DamageType, float> damages)
    {
        Stats attacker = aggression.attacker.GetComponent<Stats>();

        float totalDamage = 0;

        float defense = 0;
        float damage = 0;

        foreach (var damageRate in damages)
        {
            switch (damageRate.Key)
            {
                case Aggression.DamageType.Physical:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.Armor].Value - attacker.stats[(int)Stat.Type.ArmorPenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Attack].Value * damageRate.Value;
                    break;
                case Aggression.DamageType.Fire:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.FireResistance].Value - attacker.stats[(int)Stat.Type.FirePenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Fire].Value * damageRate.Value;
                    break;
                case Aggression.DamageType.Ice:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.IceResistance].Value - attacker.stats[(int)Stat.Type.IcePenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Ice].Value * damageRate.Value;
                    break;
                case Aggression.DamageType.Lightning:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.LightningResistance].Value - attacker.stats[(int)Stat.Type.LightningPenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Lightning].Value * damageRate.Value;
                    break;
            }
            totalDamage += Mathf.Max(damage - defense, 0);
        }

        float critChance = 0;
        float critDamage = 1;

        switch (aggression.type)
        {
            case Aggression.Type.Attack:
                critChance = aggression.attacker.GetComponent<Stats>().stats[(int)Stat.Type.AttackCriticalChance].Value;
                critDamage += aggression.attacker.GetComponent<Stats>().stats[(int)Stat.Type.AttackCriticalDamage].Value;
                break;
            case Aggression.Type.Spell:
                critChance = aggression.attacker.GetComponent<Stats>().stats[(int)Stat.Type.SpellCriticalChance].Value;
                critDamage += aggression.attacker.GetComponent<Stats>().stats[(int)Stat.Type.SpellCriticalDamage].Value;
                break;
        }

        if (Random.Range(1f, 101f) <= critChance)
        {
            totalDamage *= critDamage;
        }

        target.hp = Mathf.Max(target.hp - totalDamage, 0);
    }
}
