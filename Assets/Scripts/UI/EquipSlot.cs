using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotId;
    public int? itemId;
    public int count;

    public GameObject player;

    [SerializeField]
    GameObject movingItemSlotPrefab;

    public PlayerState state;

    public PlayerEquip equip;

    public PlayerSkillSlots skillSlots;

    [SerializeField]
    ItemDataBase itemDataBase;

    public Image image;
    public TextMeshProUGUI text;

    public Transform movingItemCanvas;

    [SerializeField]
    ItemDivideWindow itemDivideWindow;

    public void SetItem(int? id, int count)
    {
        if (id.HasValue)
        {
            itemId = id;
            this.count = count;

            image.sprite = itemDataBase.items[(int)id].icon;
            text.text = count.ToString();
        }
        else
        {
            itemId = null;
            count = 0;

            image.sprite = null;
            text.text = "";
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
            equip.Equip(equip.currentEquipedSlotIndex);
        }
        else
        {
            MovingItemSlotPrefab misp = state.movingItem;
            if (itemId == null) //빈칸
            {
                SetItem(misp.itemId, misp.count);
                Destroy(misp.gameObject);
                state.isMovingItemOnInventory = false;
                state.movingItem = null;
                equip.Equip(equip.currentEquipedSlotIndex);
            }
            else if (itemId == misp.itemId) //아이템 보충
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
                        equip.Equip(equip.currentEquipedSlotIndex);
                    }
                    else
                    {
                        count += rest;
                        misp.count -= rest;
                        equip.Equip(equip.currentEquipedSlotIndex);
                    }
                }
            }
            else //아이템 교체
            {
                int nowId = (int)itemId;
                int nowCount = count;

                misp.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();

                SetItem(misp.itemId, misp.count);

                misp.itemId = nowId;
                misp.count = nowCount;
                
                equip.Equip(equip.currentEquipedSlotIndex);
            }
        }
    }
}
