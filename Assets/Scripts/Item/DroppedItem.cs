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

    //TODO: non Monobehaviour singleton으로 바꾸기
    ItemDataBase itemDataBase;

    private void Start()
    {
        itemDataBase = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();
    }
    //===========================================

    void Update()
    {
        //이름표 위치 시작
        textBackgroundImage.rotation = Quaternion.LookRotation(textBackgroundImage.position - Camera.main.transform.position);
        textBackgroundImage.position = textBackgroundImage.forward * - 3.0f;
        //이름표 위치 끝

        //클릭 시 아이템 획득 처리
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
