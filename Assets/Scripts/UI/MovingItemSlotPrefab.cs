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

    GameObject droppedItemPrefab;

    public GameObject player;

    public PlayerInventory inventory;

    private void Awake()
    {
        droppedItemPrefab = Resources.Load<GameObject>("Prefabs/DroppedItem");
    }

    void Update()
    {
        CanvasScaler scaler = GetComponentInParent<CanvasScaler>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventory[slotId].itemId = itemId;
            inventory[slotId].count = count;
            inventory.WindowUpdate();
            player.GetComponent<PlayerState>().isMovingItemOnInventory = false;
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(0))
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        GameObject go = Instantiate(droppedItemPrefab);
        DroppedItem droppedItem = go.GetComponent<DroppedItem>();
        droppedItem.SetItem(itemId, count);

        go.transform.position = player.transform.position;

        player.GetComponent<PlayerState>().isMovingItemOnInventory = false;

        Destroy(gameObject);
    }
}
