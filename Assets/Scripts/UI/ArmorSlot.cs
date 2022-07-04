using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ArmorSlot : MonoBehaviour, IPointerClickHandler
{
    public int? itemId;

    public Sprite defaultSprite;

    public Image image;

    public Transform movingItemCanvas;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    public PlayerState state;
    public PlayerItemHandler itemHandler;

    [SerializeField]
    ItemDataBase itemDataBase;

    [SerializeField]
    ItemDivideWindow itemDivideWindow;

    public Equipment.Type type;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetItem(int? id)
    {
        if (!id.HasValue)
        {
            itemId = null;
            image.sprite = defaultSprite;
        }
        else
        {
            itemId = id;
            image.sprite = itemDataBase.items[(int)id].icon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!state.isMovingItemOnInventory)
            {
                if (itemId == null)
                {
                    return;
                }

                if (itemDivideWindow.gameObject.activeSelf)
                {
                    return;
                }

                GameObject go = Instantiate(movingItemSlotPrefab);
                MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                misp.itemId = (int)itemId;
                misp.count = 1;
                misp.transform.SetParent(movingItemCanvas);
                misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                misp.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                state.movingItem = misp;
                state.isMovingItemOnInventory = true;

                itemHandler.UnequipItem(itemId.Value);
                SetItem(null);
            }
            else
            {
                MovingItemSlotPrefab misp = state.movingItem;

                Equipment equippingItem = itemDataBase.items[misp.itemId] as Equipment;

                if (equippingItem == null)
                {
                    return;
                }

                if (equippingItem.type != type)
                {
                    return;
                }

                if (itemId == null) // 아이템 장착
                {
                    itemHandler.EquipItem(misp.itemId);
                    SetItem(misp.itemId);
                    Destroy(misp.gameObject);
                    state.isMovingItemOnInventory = false;
                    state.movingItem = null;
                }
                else //아이템 교체
                {
                    int mispId = misp.itemId;
                    itemHandler.UnequipItem(itemId.Value);
                    itemHandler.EquipItem(misp.itemId);
                    misp.itemId = itemId.Value;

                    misp.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                    SetItem(mispId);
                }
            }
        }
    }
}
