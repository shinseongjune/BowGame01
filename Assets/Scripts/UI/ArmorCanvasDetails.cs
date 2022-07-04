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
                        desc.Append("�ִ� ü��");
                        break;
                    case Stat.Type.MaxMP:
                        desc.Append("�ִ� ����");
                        break;
                    case Stat.Type.Attack:
                        desc.Append("���ݷ�");
                        break;
                    case Stat.Type.Fire:
                        desc.Append("ȭ�� ���ݷ�");
                        break;
                    case Stat.Type.Ice:
                        desc.Append("�ñ� ���ݷ�");
                        break;
                    case Stat.Type.Lightning:
                        desc.Append("���� ���ݷ�");
                        break;
                    case Stat.Type.Armor:
                        desc.Append("����");
                        break;
                    case Stat.Type.FireResistance:
                        desc.Append("ȭ�� ����");
                        break;
                    case Stat.Type.IceResistance:
                        desc.Append("�ñ� ����");
                        break;
                    case Stat.Type.LightningResistance:
                        desc.Append("���� ����");
                        break;
                    case Stat.Type.ArmorPenetration:
                        desc.Append("��� ����");
                        break;
                    case Stat.Type.FirePenetration:
                        desc.Append("ȭ�� ����");
                        break;
                    case Stat.Type.IcePenetration:
                        desc.Append("�ñ� ����");
                        break;
                    case Stat.Type.LightningPenetration:
                        desc.Append("���� ����");
                        break;
                    case Stat.Type.MovementSpeed:
                        desc.Append("�̵� �ӵ�");
                        break;
                    case Stat.Type.CooldownReduction:
                        desc.Append("��Ÿ�� ����");
                        break;
                    case Stat.Type.AttackCriticalChance:
                        desc.Append("���� ġ��Ÿ Ȯ��");
                        break;
                    case Stat.Type.AttackCriticalDamage:
                        desc.Append("���� ġ��Ÿ ����");
                        break;
                    case Stat.Type.SpellCriticalChance:
                        desc.Append("�ֹ� ġ��Ÿ Ȯ��");
                        break;
                    case Stat.Type.SpellCriticalDamage:
                        desc.Append("�ֹ� ġ��Ÿ ����");
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
