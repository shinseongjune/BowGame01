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

    public void Damaged(Aggression aggression)
    {
        Stats attacker = aggression.attacker.GetComponent<Stats>();

        float defense = 0;

        switch (aggression.type)
        {
            case Aggression.Type.Attack:
                defense = Mathf.Max(target.stats[(int)Stat.Type.Armor].Value - attacker.stats[(int)Stat.Type.ArmorPenetration].Value, 0);
                break;
            case Aggression.Type.Fire:
                defense = Mathf.Max(target.stats[(int)Stat.Type.FireResistance].Value - attacker.stats[(int)Stat.Type.FirePenetration].Value, 0);
                break;
            case Aggression.Type.Ice:
                defense = Mathf.Max(target.stats[(int)Stat.Type.IceResistance].Value - attacker.stats[(int)Stat.Type.IcePenetration].Value, 0);
                break;
            case Aggression.Type.Lightning:
                defense = Mathf.Max(target.stats[(int)Stat.Type.LightningResistance].Value - attacker.stats[(int)Stat.Type.LightningPenetration].Value, 0);
                break;
        }
        float damage = Mathf.Max(aggression.damage - defense, 0);
        target.hp = Mathf.Max(target.hp - damage, 0);
    }
}
