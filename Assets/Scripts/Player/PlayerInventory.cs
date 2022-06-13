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

    readonly List<ItemSlot> slots = new();

    private void Start()
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            slots[i] = new ItemSlot(i, null, 0);
        }
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

    public void WindowOn()
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            ItemSlot slot = slots[i];
            Image slotImage = inventory.GetChild(i).GetComponent<Image>();
            if (slot.itemId == null)
            {
                slotImage.sprite = null;
                slotImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                slotImage.sprite = itemDatabase.items[(int)slot.itemId].icon;
                slotImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slot.count.ToString();
            }
        }
    }
}
