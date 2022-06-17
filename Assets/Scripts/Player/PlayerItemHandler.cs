using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    PlayerState state;
    PlayerSkillSlots slots;

    public Transform inventory;
    public ItemDataBase itemDataBase;
    public GameObject droppedItemPrefab;

    public const float ITEM_DROP_DISTANCE = 3.5f;

    private void Start()
    {
        state = GetComponent<PlayerState>();
        slots = GetComponent<PlayerSkillSlots>();
        droppedItemPrefab = Resources.Load<GameObject>("Prefabs/DroppedItem");
    }

    public void GetItem(int id, int count)
    {
        for (int i = 0; i < inventory.childCount; i++)
        {
            ItemSlotUI itemSlot = inventory.GetChild(i).GetComponent<ItemSlotUI>();

            if (itemSlot.itemId == null)
            {
                itemSlot.SetItem(id, count);
                count = 0;
                break;
            }
            else if (itemSlot.itemId == id && itemSlot.count < itemDataBase.items[(int)itemSlot.itemId].MAX_COUNT)
            {
                int rest = itemDataBase.items[(int)itemSlot.itemId].MAX_COUNT - itemSlot.count;
                if (count <= rest)
                {
                    itemSlot.count += count;
                    itemSlot.text.text = itemSlot.count.ToString();
                    count = 0;
                    break;
                }
                else
                {
                    itemSlot.count += rest;
                    itemSlot.text.text = itemSlot.count.ToString();

                    count -= rest;
                }
            }
        }

        if (count > 0) //인벤토리에 남은 공간이 없을 경우
        {
            GameObject go = Instantiate(droppedItemPrefab);
            DroppedItem item = go.GetComponent<DroppedItem>();
            item.SetItem(id, count);
            
            go.transform.position = transform.position;
        }
    }

    public void GetItem(DroppedItem droppedItem)
    {
        for (int i = 0; i < inventory.childCount; i++)
        {
            ItemSlotUI itemSlot = inventory.GetChild(i).GetComponent<ItemSlotUI>();

            if (itemSlot.itemId == null)
            {
                itemSlot.SetItem(droppedItem.itemId, droppedItem.itemCount);
                Destroy(droppedItem.gameObject);
                break;
            }
            else if (itemSlot.itemId == droppedItem.itemId && itemSlot.count < itemDataBase.items[(int)itemSlot.itemId].MAX_COUNT)
            {
                int rest = itemDataBase.items[(int)itemSlot.itemId].MAX_COUNT - itemSlot.count;
                if (droppedItem.itemCount <= rest)
                {
                    itemSlot.count += droppedItem.itemCount;
                    itemSlot.text.text = itemSlot.count.ToString();

                    Destroy(droppedItem.gameObject);
                    break;
                }
                else
                {
                    itemSlot.count += rest;
                    itemSlot.text.text = itemSlot.count.ToString();

                    droppedItem.itemCount -= rest;
                }
            }
        }
    }

    public void DropItem(MovingItemSlotPrefab misp, RaycastHit hit)
    {
        GameObject go = Instantiate(droppedItemPrefab);
        DroppedItem item = go.GetComponent<DroppedItem>();
        item.SetItem(misp.itemId, misp.count);
        Vector3 dropPosition;
        if (Vector3.Distance(transform.position, hit.point) <= ITEM_DROP_DISTANCE)
        {
            dropPosition = hit.point;
        }
        else
        {
            dropPosition = transform.position + (hit.point - transform.position).normalized * ITEM_DROP_DISTANCE;
        }
        go.transform.position = dropPosition;

        state.isMovingItemOnInventory = false;
        state.movingItem = null;

        Destroy(misp.gameObject);
    }
}
