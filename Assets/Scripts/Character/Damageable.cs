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

    public void Damaged(Aggression aggression, Dictionary<Aggression.Type, float> damages)
    {
        Stats attacker = aggression.attacker.GetComponent<Stats>();

        float totalDamage = 0;

        float defense = 0;
        float damage = 0;

        foreach (var damageRate in damages)
        {
            switch (damageRate.Key)
            {
                case Aggression.Type.Attack:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.Armor].Value - attacker.stats[(int)Stat.Type.ArmorPenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Attack].Value * damageRate.Value;
                    break;
                case Aggression.Type.Fire:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.FireResistance].Value - attacker.stats[(int)Stat.Type.FirePenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Fire].Value * damageRate.Value;
                    break;
                case Aggression.Type.Ice:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.IceResistance].Value - attacker.stats[(int)Stat.Type.IcePenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Ice].Value * damageRate.Value;
                    break;
                case Aggression.Type.Lightning:
                    defense = Mathf.Max(target.stats[(int)Stat.Type.LightningResistance].Value - attacker.stats[(int)Stat.Type.LightningPenetration].Value, 0);
                    damage = attacker.stats[(int)Stat.Type.Lightning].Value * damageRate.Value;
                    break;
            }
            totalDamage += Mathf.Max(damage - defense, 0);
        }

        target.hp = Mathf.Max(target.hp - totalDamage, 0);
    }
}
