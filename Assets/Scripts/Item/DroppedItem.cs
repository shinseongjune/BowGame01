using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroppedItem : MonoBehaviour
{
    public int itemId;
    public int itemCount;

    [SerializeField] Transform textBackgroundImage;
    [SerializeField] MeshRenderer modelMeshRenderer;
    [SerializeField] TextMeshProUGUI itemName;

    //TODO: non Monobehaviour singleton���� �ٲٱ�
    ItemDataBase itemDataBase;

    private void Awake()
    {
        itemDataBase = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();
    }
    //===========================================

    void Update()
    {
        //�̸�ǥ ��ġ ����
        textBackgroundImage.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        textBackgroundImage.position = transform.position + textBackgroundImage.forward * - 3.0f;
        //�̸�ǥ ��ġ ��

        //Ŭ�� �� ������ ȹ�� ó��
    }

    public void SetItem(int id, int count)
    {
        itemId = id;
        itemCount = count;

        Sprite icon = itemDataBase.items[itemId].icon;
        modelMeshRenderer.material.SetTexture("_MainTex", icon.texture);
        itemName.text = itemDataBase.items[itemId].itemName;
    }
}
