using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSlot
{
    public int slotNumber;
    public int? itemId;
    public int count;

    public ItemSlot(int slotNumber, int? itemId, int count)
    {
        this.slotNumber = slotNumber;
        this.itemId = itemId;
        this.count = count;
    }
}

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    Transform inventory;

    [SerializeField]
    ItemDataBase itemDatabase;

    public const int SLOT_COUNT = 30;

    public readonly List<ItemSlot> slots = new();

    private void Start()
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            slots.Add(new ItemSlot(i, null, 0));
        }

        //TODO: 테스트용 코드 시작
        slots[0].itemId = 0;
        slots[0].count = 1;
        slots[1].itemId = 1;
        slots[1].count = 13;
        WindowUpdate();
        //테스트용 코드 끝
    }

    public ItemSlot this[int i]
    {
        get
        {
            return slots[i];
        }
        set
        {
            slots[i] = value;
        }
    }

    public void WindowUpdate()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ItemSlot slot = slots[i];
            Image slotImage = inventory.GetChild(i).GetComponent<Image>();
            if (slot.itemId == null)
            {
                slotImage.GetComponent<ItemSlotUI>().itemId = null;
                slotImage.GetComponent<ItemSlotUI>().count = 0;
                slotImage.sprite = null;
                slotImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                slotImage.GetComponent<ItemSlotUI>().itemId = slot.itemId;
                slotImage.GetComponent<ItemSlotUI>().count = slot.count;
                slotImage.sprite = itemDatabase.items[(int)slot.itemId].icon;
                slotImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slot.count.ToString();
            }
        }
    }
}
