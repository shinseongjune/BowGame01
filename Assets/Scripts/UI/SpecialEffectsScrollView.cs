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
                            sb.Append("�ִ� ü��");
                            break;
                        case Stat.Type.MaxMP:
                            sb.Append("�ִ� ����");
                            break;
                        case Stat.Type.Attack:
                            sb.Append("���ݷ�");
                            break;
                        case Stat.Type.Fire:
                            sb.Append("ȭ�� ���ݷ�");
                            break;
                        case Stat.Type.Ice:
                            sb.Append("�ñ� ���ݷ�");
                            break;
                        case Stat.Type.Lightning:
                            sb.Append("���� ���ݷ�");
                            break;
                        case Stat.Type.Armor:
                            sb.Append("����");
                            break;
                        case Stat.Type.FireResistance:
                            sb.Append("ȭ�� ����");
                            break;
                        case Stat.Type.IceResistance:
                            sb.Append("�ñ� ����");
                            break;
                        case Stat.Type.LightningResistance:
                            sb.Append("���� ����");
                            break;
                        case Stat.Type.ArmorPenetration:
                            sb.Append("��� ����");
                            break;
                        case Stat.Type.FirePenetration:
                            sb.Append("ȭ�� ����");
                            break;
                        case Stat.Type.IcePenetration:
                            sb.Append("�ñ� ����");
                            break;
                        case Stat.Type.LightningPenetration:
                            sb.Append("���� ����");
                            break;
                        case Stat.Type.MovementSpeed:
                            sb.Append("�̵� �ӵ�");
                            break;
                        case Stat.Type.CooldownReduction:
                            sb.Append("��Ÿ�� ����");
                            break;
                        case Stat.Type.AttackCriticalChance:
                            sb.Append("���� ġ��Ÿ Ȯ��");
                            break;
                        case Stat.Type.AttackCriticalDamage:
                            sb.Append("���� ġ��Ÿ ����");
                            break;
                        case Stat.Type.SpellCriticalChance:
                            sb.Append("�ֹ� ġ��Ÿ Ȯ��");
                            break;
                        case Stat.Type.SpellCriticalDamage:
                            sb.Append("�ֹ� ġ��Ÿ ����");
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
