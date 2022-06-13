using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
