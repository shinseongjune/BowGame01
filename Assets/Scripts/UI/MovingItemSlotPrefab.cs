using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovingItemSlotPrefab : MonoBehaviour
{
    public int itemId;
    public int count;
    public int slotId;

    public bool isDragging = false;

    GraphicRaycaster gr;

    GameObject droppedItemPrefab;

    [SerializeField]
    GameObject player;

    [SerializeField]
    PlayerInventory inventory;

    private void Awake()
    {
        gr = transform.root.GetComponent<GraphicRaycaster>();
        droppedItemPrefab = Resources.Load<GameObject>("Prefabs/DroppedItem");
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (!isDragging)
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            ItemSlotUI slot = results[0].gameObject.GetComponent<ItemSlotUI>();
            if (slot == null)
            {
                EquipSlot equipSlot = results[0].gameObject.GetComponent<EquipSlot>();
                if (equipSlot == null)
                {
                    DropItem();
                }
                else
                {
                    if (equipSlot.itemId == itemId)
                    {
                        equipSlot.SupplementItem(this);
                    }
                    else
                    {
                        equipSlot.ExchangeItem(this);
                    }
                }
            }
            else
            {
                if (slot.itemId == itemId)
                {
                    slot.SupplementItem(this);
                }
                else
                {
                    slot.ExchangeItem(this);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            ItemSlotUI slot = results[0].gameObject.GetComponent<ItemSlotUI>();
            if (slot == null)
            {
                EquipSlot equipSlot = results[0].gameObject.GetComponent<EquipSlot>();
                if (equipSlot == null)
                {
                    DropItem();
                }
                else
                {
                    if (equipSlot.itemId == itemId)
                    {
                        equipSlot.SupplementItem(this);
                    }
                    else
                    {
                        equipSlot.ExchangeItem(this);
                    }
                }
            }
            else
            {
                if (slot.itemId == itemId)
                {
                    slot.SupplementItem(this);
                }
                else
                {
                    slot.ExchangeItem(this);
                }
            }
        }
    }

    public void DropItem()
    {
        GameObject go = Instantiate(droppedItemPrefab);
        DroppedItem droppedItem = go.GetComponent<DroppedItem>();
        droppedItem.SetItem(itemId, count);

        go.transform.position = player.transform.position;

        inventory[slotId].itemId = null;
        inventory[slotId].count = 0;

        Destroy(gameObject);

        inventory.WindowUpdate();
    }
}
