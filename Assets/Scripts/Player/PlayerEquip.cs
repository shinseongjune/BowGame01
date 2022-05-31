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
        //TODO: delete this. �κ��丮�� ������ ���� �� ��������Ʈ�� �ٲٵ���.
        //Debug�� �ڵ� ����
        equipment[0] = 0;
        equipSlots.transform.GetChild(0).GetComponent<Image>().sprite = itemDataBase.items[(int)equipment[0]].icon;
        //Debug�� �ڵ� ��
    }

    private void Update()
    {
        //����Ű �Է� ����
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
        //����Ű �Է� ��
    }
}
