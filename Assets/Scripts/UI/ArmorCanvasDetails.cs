using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ArmorCanvasDetails : MonoBehaviour
{
    [SerializeField]
    Stats stats;

    public Transform content;

    [SerializeField]
    GameObject detailsContentPrefab;

    public void OnOff()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            UpdateContent();
        }
    }

    public void UpdateContent()
    {
        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }

        foreach(Stat stat in stats.stats)
        {
            GameObject go = Instantiate(detailsContentPrefab, content);
            TextMeshProUGUI text = go.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = string.Format("{0} : {1}", stat.name, stat.Value);

            DetailsContentPrefab dcp = go.GetComponent<DetailsContentPrefab>();
            dcp.description = stat.description;
        }

        foreach(SpecialEffect effect in stats.specialEffects)
        {
            GameObject go = Instantiate(detailsContentPrefab, content);
            TextMeshProUGUI text = go.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = effect.name;

            DetailsContentPrefab dcp = go.GetComponent<DetailsContentPrefab>();
            StringBuilder desc = new(string.Format("{0}\n------------\n", effect.description));

            foreach (var mods in effect.modifiers)
            {
                switch (mods.Key)
                {
                    case Stat.Type.MaxHP:
                        desc.Append("최대 체력");
                        break;
                    case Stat.Type.MaxMP:
                        desc.Append("최대 마력");
                        break;
                    case Stat.Type.Attack:
                        desc.Append("공격력");
                        break;
                    case Stat.Type.Fire:
                        desc.Append("화염 공격력");
                        break;
                    case Stat.Type.Ice:
                        desc.Append("냉기 공격력");
                        break;
                    case Stat.Type.Lightning:
                        desc.Append("번개 공격력");
                        break;
                    case Stat.Type.Armor:
                        desc.Append("방어력");
                        break;
                    case Stat.Type.FireResistance:
                        desc.Append("화염 저항");
                        break;
                    case Stat.Type.IceResistance:
                        desc.Append("냉기 저항");
                        break;
                    case Stat.Type.LightningResistance:
                        desc.Append("번개 저항");
                        break;
                    case Stat.Type.ArmorPenetration:
                        desc.Append("방어 관통");
                        break;
                    case Stat.Type.FirePenetration:
                        desc.Append("화염 관통");
                        break;
                    case Stat.Type.IcePenetration:
                        desc.Append("냉기 관통");
                        break;
                    case Stat.Type.LightningPenetration:
                        desc.Append("번개 관통");
                        break;
                    case Stat.Type.MovementSpeed:
                        desc.Append("이동 속도");
                        break;
                    case Stat.Type.CooldownReduction:
                        desc.Append("쿨타임 감소");
                        break;
                    case Stat.Type.AttackCriticalChance:
                        desc.Append("공격 치명타 확률");
                        break;
                    case Stat.Type.AttackCriticalDamage:
                        desc.Append("공격 치명타 피해");
                        break;
                    case Stat.Type.SpellCriticalChance:
                        desc.Append("주문 치명타 확률");
                        break;
                    case Stat.Type.SpellCriticalDamage:
                        desc.Append("주문 치명타 피해");
                        break;
                }

                desc.Append(" : ");
                switch (mods.Value.type)
                {
                    case StatModifier.Type.BaseFlat:
                        desc.Append("+");
                        break;
                    case StatModifier.Type.BaseMult:
                        desc.Append("x");
                        break;
                    case StatModifier.Type.TotalFlat:
                        desc.Append("+");
                        break;
                    case StatModifier.Type.TotalMultA:
                    case StatModifier.Type.TotalMultB:
                    case StatModifier.Type.TotalMultC:
                    case StatModifier.Type.TotalMultD:
                        desc.Append("x");
                        break;
                }
                desc.Append(mods.Value.value);
                desc.Append("\n");
            }

            dcp.description = desc.ToString();
        }
    }
}
