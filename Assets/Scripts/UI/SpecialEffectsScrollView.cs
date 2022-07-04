using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEffectsScrollView : MonoBehaviour
{
    [SerializeField]
    Stats stats;

    [SerializeField]
    Transform content;

    [SerializeField]
    GameObject specialEffectIconPrefab;

    [SerializeField]
    GameObject descriptionWindow;

    void Update()
    {
        if (stats.isSpecialEffectsUpdated)
        {
            for(int i = content.childCount - 1; i >= 0; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            descriptionWindow.SetActive(false);

            foreach (SpecialEffect effect in stats.specialEffects)
            {
                GameObject go = Instantiate(specialEffectIconPrefab, content);
                go.GetComponent<Image>().sprite = effect.icon;
                SpecialEffectIconPrefab seip = go.GetComponent<SpecialEffectIconPrefab>();
                StringBuilder sb = new(effect.name);
                sb.Append("\n");
                sb.Append(effect.description);
                sb.Append("\n----------\n");

                foreach (var mods in effect.modifiers)
                {
                    switch (mods.Key)
                    {
                        case Stat.Type.MaxHP:
                            sb.Append("최대 체력");
                            break;
                        case Stat.Type.MaxMP:
                            sb.Append("최대 마력");
                            break;
                        case Stat.Type.Attack:
                            sb.Append("공격력");
                            break;
                        case Stat.Type.Fire:
                            sb.Append("화염 공격력");
                            break;
                        case Stat.Type.Ice:
                            sb.Append("냉기 공격력");
                            break;
                        case Stat.Type.Lightning:
                            sb.Append("번개 공격력");
                            break;
                        case Stat.Type.Armor:
                            sb.Append("방어력");
                            break;
                        case Stat.Type.FireResistance:
                            sb.Append("화염 저항");
                            break;
                        case Stat.Type.IceResistance:
                            sb.Append("냉기 저항");
                            break;
                        case Stat.Type.LightningResistance:
                            sb.Append("번개 저항");
                            break;
                        case Stat.Type.ArmorPenetration:
                            sb.Append("방어 관통");
                            break;
                        case Stat.Type.FirePenetration:
                            sb.Append("화염 관통");
                            break;
                        case Stat.Type.IcePenetration:
                            sb.Append("냉기 관통");
                            break;
                        case Stat.Type.LightningPenetration:
                            sb.Append("번개 관통");
                            break;
                        case Stat.Type.MovementSpeed:
                            sb.Append("이동 속도");
                            break;
                        case Stat.Type.CooldownReduction:
                            sb.Append("쿨타임 감소");
                            break;
                        case Stat.Type.AttackCriticalChance:
                            sb.Append("공격 치명타 확률");
                            break;
                        case Stat.Type.AttackCriticalDamage:
                            sb.Append("공격 치명타 피해");
                            break;
                        case Stat.Type.SpellCriticalChance:
                            sb.Append("주문 치명타 확률");
                            break;
                        case Stat.Type.SpellCriticalDamage:
                            sb.Append("주문 치명타 피해");
                            break;
                    }

                    sb.Append(" : ");
                    switch (mods.Value.type)
                    {
                        case StatModifier.Type.BaseFlat:
                            sb.Append("+");
                            break;
                        case StatModifier.Type.BaseMult:
                            sb.Append("x");
                            break;
                        case StatModifier.Type.TotalFlat:
                            sb.Append("+");
                            break;
                        case StatModifier.Type.TotalMultA:
                        case StatModifier.Type.TotalMultB:
                        case StatModifier.Type.TotalMultC:
                        case StatModifier.Type.TotalMultD:
                            sb.Append("x");
                            break;
                    }
                    sb.Append(mods.Value.value);
                    sb.Append("\n");
                }


                seip.description = sb.ToString();
            }

            stats.isSpecialEffectsUpdated = false;
        }
    }
}
