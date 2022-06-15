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
    ItemDataBase itemDataBase;

    [SerializeField]
    GameObject player;

    Image image;
    TextMeshProUGUI text;

    [SerializeField]
    Transform movingItemCanvas;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(int? itemId, int count)
    {
        if (itemId == null)
        {
            this.itemId = null;
            this.count = 0;

            image.sprite = null;
            text.text = "";
        }
        else
        {
            this.itemId = itemId;
            this.count = count;

            image.sprite = itemDataBase.items[(int)itemId].icon;
            text.text = count.ToString();
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
            misp.player = player;
            misp.transform.SetParent(movingItemCanvas);
            misp.image.sprite = GetComponent<Image>().sprite;
            misp.text.text = count.ToString();
            state.movingItem = misp;
            state.isMovingItemOnInventory = true;

            SetItem(null, 0);
        }
        else
        {
            MovingItemSlotPrefab misp = state.movingItem;
            if (itemId == null)
            {
                SetItem(misp.itemId, count);
                Destroy(misp.gameObject);
                state.isMovingItemOnInventory = false;
                state.movingItem = null;
            }
            else if (itemId == misp.itemId)
            {
                Item item = itemDataBase.items[(int)itemId];
                if (count >= item.MAX_COUNT)
                {
                    return;
                }
                else
                {
                    int rest = item.MAX_COUNT - count;

                    if (misp.count <= rest)
                    {
                        count += misp.count;
                        Destroy(misp.gameObject);
                        state.isMovingItemOnInventory = false;
                        state.movingItem = null;
                    }
                    else
                    {
                        count += rest;
                        misp.count -= rest;
                    }
                }
            }
            else
            {
                int nowId = (int)itemId;
                int nowCount = count;

                misp.image.sprite = GetComponent<Image>().sprite;
                misp.text.text = count.ToString();

                SetItem(misp.itemId, misp.count);

                misp.itemId = nowId;
                misp.count = nowCount;
            }
        }
    }
}
