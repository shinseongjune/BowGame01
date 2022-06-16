using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class ItemDivideWindow : MonoBehaviour
{
    public GameObject slot;

    public TMP_InputField inputField;

    public GameObject movingItemSlotPrefab;

    public Transform inventory;

    public Transform movingItemCanvas;

    private void Start()
    {
        inputField.characterLimit = 10;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }

    public void InitializeWindow(GameObject slot)
    {
        this.slot = slot;
        inputField.text = "";
    }

    public void OK()
    {
        if (inputField.text.Length <= 0)
        {
            return;
        }

        int num = int.Parse(inputField.text);

        ItemSlotUI itemSlot = slot.GetComponent<ItemSlotUI>();

        if (itemSlot == null)
        {
            EquipSlot equipSlot = slot.GetComponent<EquipSlot>();

            if (equipSlot.count < num)
            {
                GameObject go = Instantiate(movingItemSlotPrefab);
                MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                misp.itemId = (int)equipSlot.itemId;
                misp.count = equipSlot.count;
                misp.transform.SetParent(equipSlot.movingItemCanvas);
                misp.GetComponent<Image>().sprite = equipSlot.GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = equipSlot.count.ToString();
                misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                equipSlot.state.movingItem = misp;
                equipSlot.state.isMovingItemOnInventory = true;

                equipSlot.SetItem(null, 0);
                equipSlot.equip.Equip(equipSlot.equip.currentEquipedSlotIndex);
            }
            else
            {
                GameObject go = Instantiate(movingItemSlotPrefab);
                MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                misp.itemId = (int)equipSlot.itemId;
                misp.count = num;
                misp.transform.SetParent(equipSlot.movingItemCanvas);
                misp.GetComponent<Image>().sprite = slot.GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = num.ToString();
                misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                equipSlot.state.movingItem = misp;
                equipSlot.state.isMovingItemOnInventory = true;

                equipSlot.count -= num;
                equipSlot.text.text = equipSlot.count.ToString();
                equipSlot.equip.Equip(equipSlot.equip.currentEquipedSlotIndex);
            }
        }
        else
        {
            if (itemSlot.count < num)
            {
                GameObject go = Instantiate(movingItemSlotPrefab);
                MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                misp.itemId = (int)itemSlot.itemId;
                misp.count = itemSlot.count;
                misp.transform.SetParent(itemSlot.movingItemCanvas);
                misp.GetComponent<Image>().sprite = itemSlot.GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = itemSlot.count.ToString();
                misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                itemSlot.state.movingItem = misp;
                itemSlot.state.isMovingItemOnInventory = true;

                itemSlot.SetItem(null, 0);
            }
            else
            {
                GameObject go = Instantiate(movingItemSlotPrefab);
                MovingItemSlotPrefab misp = go.GetComponent<MovingItemSlotPrefab>();
                misp.itemId = (int)itemSlot.itemId;
                misp.count = num;
                misp.transform.SetParent(itemSlot.movingItemCanvas);
                misp.GetComponent<Image>().sprite = slot.GetComponent<Image>().sprite;
                misp.GetComponentInChildren<TextMeshProUGUI>().text = num.ToString();
                misp.movingItemCanvas = movingItemCanvas.GetComponent<Canvas>();
                itemSlot.state.movingItem = misp;
                itemSlot.state.isMovingItemOnInventory = true;

                itemSlot.count -= num;
                itemSlot.text.text = itemSlot.count.ToString();
            }
        }

        

        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
