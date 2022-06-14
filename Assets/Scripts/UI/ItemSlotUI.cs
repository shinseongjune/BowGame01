using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    public int slotId;
    public int? itemId;
    public int count;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    [SerializeField]
    PlayerState state;

    [SerializeField]
    PlayerInventory inventory;

    [SerializeField]
    ItemDataBase itemDataBase;

    [SerializeField]
    GameObject player;

    public void ExchangeItem(MovingItemSlotPrefab movingItem)
    {
        GameObject go = Instantiate(movingItemSlotPrefab);
        go.GetComponent<MovingItemSlotPrefab>().itemId = (int)itemId;
        go.GetComponent<MovingItemSlotPrefab>().count = count;
        go.GetComponent<MovingItemSlotPrefab>().slotId = slotId;
        Image image = go.GetComponent<Image>();
        image.sprite = image.sprite;
        state.isMovingItemOnInventory = true;

        inventory[slotId].itemId = movingItem.itemId;
        inventory[slotId].count = movingItem.count;

        Destroy(movingItem.gameObject);
        inventory.WindowUpdate();
    }

    public void SupplementItem(MovingItemSlotPrefab movingItem)
    {
        Item item = itemDataBase.items[(int)itemId];
        if (count >= item.MAX_COUNT)
        {
            return;
        }
        else
        {
            int rest = item.MAX_COUNT - count;

            if (movingItem.count <= rest)
            {
                inventory[slotId].count += rest;
                count += rest;
                Destroy(movingItem.gameObject);
            }
            else
            {
                inventory[slotId].count += rest;
                count += rest;
                movingItem.count -= rest;
                movingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingItem.count.ToString();
            }
            inventory.WindowUpdate();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!state.isMovingItemOnInventory)
        {
            if (itemId == null)
            {
                return;
            }

            GameObject go = Instantiate(movingItemSlotPrefab);
            MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
            misp.itemId = (int)itemId;
            misp.count = count;
            misp.slotId = slotId;
            misp.player = player;
            misp.inventory = player.GetComponent<PlayerInventory>();
            misp.transform.SetParent(transform.root);
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();
            Image image = go.GetComponent<Image>();
            image.sprite = GetComponent<Image>().sprite;
            state.isMovingItemOnInventory = true;

            itemId = null;
            count = 0;
            inventory[slotId].itemId = null;
            inventory[slotId].count = 0;
            inventory.WindowUpdate();
        }
    }
}
