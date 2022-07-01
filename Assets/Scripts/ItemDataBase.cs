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
        //TODO: 파일 읽기로 해야할 것 같다. 딕셔너리로 바꿀것.
        //Debug: 임시 아이템 데이터
        Item bow = new();
        bow.itemName = "단궁";
        bow.description = "나무로 만든 단궁이다.";
        bow.MAX_COUNT = 1;
        bow.icon = Resources.Load<Sprite>("Images/ShortBow");
        bow.defaultSkill = 1;
        bow.dropRate = 0.2f;
        bow.dropCount = 1;
        items.Add(bow);

        Item wood = new();
        wood.itemName = "목재";
        wood.description = "목재다.";
        wood.MAX_COUNT = 30;
        wood.icon = Resources.Load<Sprite>("Images/Wood");
        wood.dropRate = 0.8f;
        wood.dropCount = 10;
        items.Add(wood);

        Item stone = new();
        stone.itemName = "석재";
        stone.description = "석재다.";
        stone.MAX_COUNT = 30;
        stone.icon = Resources.Load<Sprite>("Images/Stone");
        stone.dropRate = 0.7f;
        stone.dropCount = 8;
        items.Add(stone);

        Equipment helmet = new();
        helmet.itemName = "헬멧";
        helmet.description = "머리에 쓰는 헬멧이다.";
        helmet.MAX_COUNT = 1;
        helmet.icon = Resources.Load<Sprite>("Images/Helmet");
        helmet.dropRate = 0.1f;
        helmet.dropCount = 1;
        helmet.type = Equipment.Type.Head;
        helmet.stats.Add(Stat.Type.Armor, 10f);
        SpecialEffect helmetEffect = new();
        helmetEffect.name = "안전 제일";
        helmetEffect.description = "머리를 든든히 보호합시다.";
        helmetEffect.isDisplayed = true;
        helmetEffect.isUnique = true;
        StatModifier helmetEffectMod = new(StatModifier.Type.TotalFlat, 5, helmetEffect);
        helmetEffect.modifiers.Add(Stat.Type.Armor, helmetEffectMod);
        helmetEffect.source = helmetEffect;
        helmet.effects.Add(helmetEffect);
        //Debug 임시 아이템 데이터 끝
    }
}
