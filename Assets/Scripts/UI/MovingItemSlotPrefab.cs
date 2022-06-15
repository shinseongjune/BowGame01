using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MovingItemSlotPrefab : MonoBehaviour
{
    public int itemId;
    public int count;

    GameObject droppedItemPrefab;

    public GameObject player;

    public Image image;
    public TextMeshProUGUI text;
    
    public Canvas movingItemCanvas;

    private void Awake()
    {
        droppedItemPrefab = Resources.Load<GameObject>("Prefabs/DroppedItem");
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ExchangeItem(int itemId, int count)
    {
        this.itemId = itemId;
        this.count = count;
    }

    void Update()
    {
        movingItemCanvas = transform.root.GetComponent<Canvas>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(movingItemCanvas.transform as RectTransform, Input.mousePosition, movingItemCanvas.worldCamera, out pos);
        transform.position = movingItemCanvas.transform.TransformPoint(pos);
        if (Input.GetMouseButtonDown(0))
        {
            //아이템 드랍 시작
            //아이템 드랍 끝
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
