using UnityEngine;
using UnityEngine.UI;

public class PlayerEquip : MonoBehaviour
{
    [SerializeField]
    ItemDataBase itemDataBase;

    public GameObject equipSlots;
    public int?[] equipment = new int?[10];

    public int currentEquipedSlotIndex = 0;

    private void Start()
    {
        //TODO: delete this. 인벤토리에 변동이 있을 시 스프라이트를 바꾸도록.
        //Debug용 코드 시작
        equipment[0] = 0;
        equipSlots.transform.GetChild(0).GetComponent<Image>().sprite = itemDataBase.items[(int)equipment[0]].icon;
        //Debug용 코드 끝
    }

    private void Update()
    {
        //숫자키 입력 시작
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentEquipedSlotIndex = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentEquipedSlotIndex = 1;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentEquipedSlotIndex = 2;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentEquipedSlotIndex = 3;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentEquipedSlotIndex = 4;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentEquipedSlotIndex = 5;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentEquipedSlotIndex = 6;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentEquipedSlotIndex = 7;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentEquipedSlotIndex = 8;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentEquipedSlotIndex = 9;
            for (int i = 0; i < 10; i++)
            {
                if (i == currentEquipedSlotIndex)
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    equipSlots.transform.GetChild(i).GetComponent<Outline>().enabled = false;
                }
            }
        }
        //숫자키 입력 끝
    }
}
