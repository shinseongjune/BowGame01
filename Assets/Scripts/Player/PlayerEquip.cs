using UnityEngine;
using UnityEngine.UI;

public class PlayerEquip : MonoBehaviour
{
    [SerializeField]
    ItemDataBase itemDataBase;

    [SerializeField]
    SkillDataBase skillDataBase;

    PlayerSkillSlots skillSlots;

    public Transform equipSlots;

    public int currentEquipedSlotIndex = 0;

    private void Start()
    {
        skillSlots = GetComponent<PlayerSkillSlots>();

        //TODO: 테스트용 코드 시작
        EquipSlot slot = equipSlots.GetChild(0).GetComponent<EquipSlot>();
        slot.SetItem(0, 1);
        Equip(0);
        //테스트용 코드 끝
    }

    private void Update()
    {
        //숫자키 입력 시작
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Equip(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Equip(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Equip(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Equip(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Equip(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Equip(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Equip(9);
        }
        //숫자키 입력 끝
    }

    void Equip(int index)
    {
        currentEquipedSlotIndex = index;
        for (int i = 0; i < 10; i++)
        {
            if (i == currentEquipedSlotIndex)
            {
                equipSlots.GetChild(i).GetComponent<Outline>().enabled = true;
            }
            else
            {
                equipSlots.GetChild(i).GetComponent<Outline>().enabled = false;
            }
        }

        EquipSlot slot = equipSlots.GetChild(currentEquipedSlotIndex).GetComponent<EquipSlot>();
        if (slot.itemId.HasValue)
        {
            Item item = itemDataBase.items[(int)slot.itemId];
            if (item.defaultSkill.HasValue)
            {
                skillSlots.defaultSkill = (int)item.defaultSkill;
            }
            else
            {
                skillSlots.defaultSkill = 0;
            }
        }
        else
        {
            skillSlots.defaultSkill = 0;
        }
    }
}
