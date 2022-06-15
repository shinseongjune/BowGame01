using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSlot : MonoBehaviour
{
    public int slotId;
    public int? itemId;
    public int count;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    [SerializeField]
    PlayerState state;

    [SerializeField]
    PlayerEquip equip;

    [SerializeField]
    PlayerSkillSlots skillSlots;

    [SerializeField]
    ItemDataBase itemDataBase;

    public void SetItem(int? id, int count)
    {
        if (id.HasValue)
        {
            itemId = id;
            this.count = count;

            Image image = GetComponent<Image>();
            image.sprite = itemDataBase.items[(int)id].icon;
            TextMeshProUGUI text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = count.ToString();
        }
        else
        {
            itemId = null;
            count = 0;

            Image image = GetComponent<Image>();
            image.sprite = null;
            TextMeshProUGUI text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = "";
        }
    }

    public void ExchangeItem(MovingItemSlotPrefab movingItem)
    {
        GameObject go = Instantiate(movingItemSlotPrefab);
        go.GetComponent<MovingItemSlotPrefab>().itemId = (int)itemId;
        go.GetComponent<MovingItemSlotPrefab>().count = count;
        Image image = go.GetComponent<Image>();
        image.sprite = image.sprite;
        state.isMovingItemOnInventory = true;

        itemId = movingItem.itemId;
        count = movingItem.count;

        Destroy(movingItem.gameObject);
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
                count += rest;
                Destroy(movingItem.gameObject);
            }
            else
            {
                count += rest;
                movingItem.count -= rest;
                movingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingItem.count.ToString();
            }
        }
    }
}
