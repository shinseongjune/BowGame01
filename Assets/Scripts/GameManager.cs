using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton start
    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Singleton end

    [SerializeField]
    Transform inventory;

    void Start()
    {
        //인벤토리 초기화 시작
        for (int i = 0; i < inventory.childCount; i++)
        {
            inventory.GetChild(i).GetComponent<ItemSlotUI>().SetItem(null, 0);
        }
        //인벤토리 초기화 끝

        //TODO: 테스트용 코드
        inventory.GetChild(0).GetComponent<ItemSlotUI>().SetItem(0, 1);
        inventory.GetChild(1).GetComponent<ItemSlotUI>().SetItem(1, 15);
        //테스트용 코드 끝
    }

    void Update()
    {
        
    }
}
