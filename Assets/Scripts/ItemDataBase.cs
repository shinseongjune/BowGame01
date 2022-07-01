using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    //Singleton start
    private static ItemDataBase instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Singleton end

    public List<Item> items = new();

    void Start()
    {
        //TODO: ���� �б�� �ؾ��� �� ����. ��ųʸ��� �ٲܰ�.
        //Debug: �ӽ� ������ ������
        Item bow = new();
        bow.itemName = "�ܱ�";
        bow.description = "������ ���� �ܱ��̴�.";
        bow.MAX_COUNT = 1;
        bow.icon = Resources.Load<Sprite>("Images/ShortBow");
        bow.defaultSkill = 1;
        bow.dropRate = 0.2f;
        bow.dropCount = 1;
        items.Add(bow);

        Item wood = new();
        wood.itemName = "����";
        wood.description = "�����.";
        wood.MAX_COUNT = 30;
        wood.icon = Resources.Load<Sprite>("Images/Wood");
        wood.dropRate = 0.8f;
        wood.dropCount = 10;
        items.Add(wood);

        Item stone = new();
        stone.itemName = "����";
        stone.description = "�����.";
        stone.MAX_COUNT = 30;
        stone.icon = Resources.Load<Sprite>("Images/Stone");
        stone.dropRate = 0.7f;
        stone.dropCount = 8;
        items.Add(stone);

        Equipment helmet = new();
        helmet.itemName = "���";
        helmet.description = "�Ӹ��� ���� ����̴�.";
        helmet.MAX_COUNT = 1;
        helmet.icon = Resources.Load<Sprite>("Images/Helmet");
        helmet.dropRate = 0.1f;
        helmet.dropCount = 1;
        helmet.type = Equipment.Type.Head;
        helmet.stats.Add(Stat.Type.Armor, 10f);
        SpecialEffect helmetEffect = new();
        helmetEffect.name = "���� ����";
        helmetEffect.description = "�Ӹ��� ����� ��ȣ�սô�.";
        helmetEffect.isDisplayed = true;
        helmetEffect.isUnique = true;
        StatModifier helmetEffectMod = new(StatModifier.Type.TotalFlat, 5, helmetEffect);
        helmetEffect.modifiers.Add(Stat.Type.Armor, helmetEffectMod);
        helmetEffect.source = helmetEffect;
        helmet.effects.Add(helmetEffect);
        //Debug �ӽ� ������ ������ ��
    }
}
