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

    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] SkillDataBase skillDataBase;
    [SerializeField] BuildingDataBase buildingDataBase;
    [SerializeField] GridManager gridManager;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Transform inventory;
    [SerializeField] MovementSkillSlot movementSkillSlot;

    [SerializeField] PlayerSkills playerSkills;

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

        movementSkillSlot.SetSkill(0);

        //playerSkills.basicSkills.Add(0);
        //playerSkills.basicSkills.Add(1);
        //playerSkills.basicSkills.Add(2);

        playerSkills.movementSkills.Add(0);
        playerSkills.movementSkills.Add(1);
        //테스트용 코드 끝

        mapGenerator.GenerateMap();
        gridManager.GenerateGrid();
    }
}
