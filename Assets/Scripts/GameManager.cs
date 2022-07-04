using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton start
    private static GameManager instance = null;

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

    [SerializeField] GameObject player;

    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] SkillDataBase skillDataBase;
    [SerializeField] BuildingDataBase buildingDataBase;
    [SerializeField] GridManager gridManager;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Transform inventory;
    [SerializeField] MovementSkillSlot movementSkillSlot;
    [SerializeField] BasicSkillSlot qBasicSkillSlot;
    [SerializeField] BasicSkillSlot eBasicSkillSlot;

    [SerializeField] PlayerSkills playerSkills;

    [SerializeField] Light directionalLight;

    public float currentTime;

    public const float DAY_NIGHT_PERIOD = 60f * 10f;

    void Start()
    {
        //인벤토리 초기화 시작
        for (int i = 0; i < inventory.childCount; i++)
        {
            ItemSlotUI slot = inventory.GetChild(i).GetComponent<ItemSlotUI>();
            if (slot == null)
            {
                continue;
            }
            slot.SetItem(null, 0);
        }
        //인벤토리 초기화 끝

        //TODO: 테스트용 코드
        inventory.GetChild(0).GetComponent<ItemSlotUI>().SetItem(0, 1);
        inventory.GetChild(1).GetComponent<ItemSlotUI>().SetItem(1, 15);
        inventory.GetChild(2).GetComponent<ItemSlotUI>().SetItem(1, 8);
        inventory.GetChild(3).GetComponent<ItemSlotUI>().SetItem(1, 25);
        inventory.GetChild(4).GetComponent<ItemSlotUI>().SetItem(2, 10);
        inventory.GetChild(5).GetComponent<ItemSlotUI>().SetItem(3, 1);
        inventory.GetChild(6).GetComponent<ItemSlotUI>().SetItem(3, 1);

        movementSkillSlot.SetSkill(1);
        qBasicSkillSlot.SetSkill(0);
        eBasicSkillSlot.SetSkill(1);

        playerSkills.basicSkills.Add(0);
        playerSkills.basicSkills.Add(1);
        //playerSkills.basicSkills.Add(2);

        playerSkills.movementSkills.Add(0);
        playerSkills.movementSkills.Add(1);
        //테스트용 코드 끝

        mapGenerator.GenerateMap();
        gridManager.GenerateGrid();

        Stats playerStat = player.GetComponent<Stats>();

        playerStat.characterId = -1;
        playerStat.stats[(int)Stat.Type.MaxHP].baseValue = 200;
        playerStat.hp = 200;
        playerStat.stats[(int)Stat.Type.MaxMP].baseValue = 100;
        playerStat.mp = 100;
        playerStat.stats[(int)Stat.Type.Attack].baseValue = 10;
        playerStat.stats[(int)Stat.Type.Fire].baseValue = 8;
        playerStat.stats[(int)Stat.Type.Ice].baseValue = 8;
        playerStat.stats[(int)Stat.Type.Lightning].baseValue = 10;
        playerStat.stats[(int)Stat.Type.Armor].baseValue = 10;
        playerStat.stats[(int)Stat.Type.FireResistance].baseValue = 5;
        playerStat.stats[(int)Stat.Type.IceResistance].baseValue = 5;
        playerStat.stats[(int)Stat.Type.LightningResistance].baseValue = 5;
        playerStat.stats[(int)Stat.Type.ArmorPenetration].baseValue = 0;
        playerStat.stats[(int)Stat.Type.FirePenetration].baseValue = 0;
        playerStat.stats[(int)Stat.Type.IcePenetration].baseValue = 0;
        playerStat.stats[(int)Stat.Type.LightningPenetration].baseValue = 0;
        playerStat.stats[(int)Stat.Type.MovementSpeed].baseValue = 0;
        playerStat.stats[(int)Stat.Type.CooldownReduction].baseValue = 0;
        playerStat.stats[(int)Stat.Type.AttackCriticalChance].baseValue = 0;
        playerStat.stats[(int)Stat.Type.AttackCriticalDamage].baseValue = 0;
        playerStat.stats[(int)Stat.Type.SpellCriticalChance].baseValue = 0;
        playerStat.stats[(int)Stat.Type.SpellCriticalDamage].baseValue = 0;

        currentTime = 60f * 3f;
        directionalLight.transform.eulerAngles = new Vector3(currentTime * 0.3f, 0, 0);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= DAY_NIGHT_PERIOD * 2)
        {
            currentTime -= DAY_NIGHT_PERIOD * 2;
        }
        directionalLight.transform.eulerAngles = new Vector3(currentTime * 0.3f, 0, 0);

    }
}
