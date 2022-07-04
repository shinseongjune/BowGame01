using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    public int? itemId;
    public int count;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    public PlayerState state;
    public PlayerItemHandler playerItemHandler;

    [SerializeField]
    ItemDataBase itemDataBase;

    Image image;
    public TextMeshProUGUI text;

    public Transform movingItemCanvas;

    [SerializeField]
    ItemDivideWindow itemDivideWindow;

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
        else if (count == 0)
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

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    itemDivideWindow.gameObject.SetActive(true);
                    itemDivideWindow.InitializeWindow(gameObject);
                }
                else
                {
                    GameObject go = Instantiate(movingItemSlotPrefab);
                    MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                    misp.itemId = (int)itemId;
                    misp.count = count;
                    misp.transform.SetParent(movingItemCanvas);
                    misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                    misp.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                    misp.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
                    state.movingItem = misp;
                    state.isMovingItemOnInventory = true;

                    SetItem(null, 0);
                }
            }
            else
            {
                MovingItemSlotPrefab misp = state.movingItem;
                if (itemId == null) //��ĭ
                {
                    SetItem(misp.itemId, misp.count);
                    Destroy(misp.gameObject);
                    state.isMovingItemOnInventory = false;
                    state.movingItem = null;
                }
                else if (itemId == misp.itemId) //������ ����
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
                            text.text = count.ToString();
                            state.isMovingItemOnInventory = false;
                            state.movingItem = null;
                        }
                        else
                        {
                            count += rest;
                            text.text = count.ToString();
                            misp.count -= rest;
                            misp.GetComponentInChildren<TextMeshProUGUI>().text = misp.count.ToString();
                        }
                    }
                }
                else //������ ��ü
                {
                    int nowId = (int)itemId;
                    int nowCount = count;

                    misp.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                    misp.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();

                    SetItem(misp.itemId, misp.count);

                    misp.itemId = nowId;
                    misp.count = nowCount;
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemId.HasValue)
            {
                if (itemDataBase.items[(int)itemId] is Consumable)
                {
                    UseItem();
                }
                else if (itemDataBase.items[(int)itemId] is Equipment)
                {
                    EquipItem();
                }
            }
        }
    }

    public void UseItem()
    {
        Consumable item = itemDataBase.items[(int)itemId] as Consumable;
        if (item != null)
        {
            item.Consume();
            count--;
            if (count <= 0)
            {
                SetItem(null, 0);
            }
        }
    }

    public void EquipItem()
    {
        playerItemHandler.EquipItem(this);
    }
}
