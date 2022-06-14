using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public int slotId;
    public int? itemId;
    public int count;

    GraphicRaycaster gr;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    [SerializeField]
    PlayerState state;

    [SerializeField]
    PlayerInventory inventory;

    [SerializeField]
    ItemDataBase itemDataBase;

    private void Awake()
    {
        gr = transform.root.GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !state.isMovingItemOnInventory)
        {
            if (itemId == null)
            {
                return;
            }

            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results[0].gameObject.Equals(gameObject))
            {
                GameObject go = Instantiate(movingItemSlotPrefab);
                go.GetComponent<MovingItemSlotPrefab>().itemId = (int)itemId;
                go.GetComponent<MovingItemSlotPrefab>().count = count;
                go.GetComponent<MovingItemSlotPrefab>().slotId = slotId;
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();
                Image image = go.GetComponent<Image>();
                image.sprite = image.sprite;
                state.isMovingItemOnInventory = true;
            }
        }
    }

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
}
