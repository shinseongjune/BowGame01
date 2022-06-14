using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    //Singleton start
    private static ItemDataBase instance = null;

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

    public Item[] items = new Item[50];

    void Start()
    {
        //TODO: 파일 읽기로 해야할 것 같다. 딕셔너리로 바꿀것.
        //Debug: 임시 아이템 데이터
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item();
            items[i].id = i;
        }
        items[0].itemName = "단궁";
        items[0].description = "나무로 만든 단궁이다.";
        items[0].MAX_COUNT = 1;
        items[0].icon = Resources.Load<Sprite>("Images/ShortBow");
        items[0].defaultSkill = 1;

        items[1].itemName = "목재";
        items[1].description = "목재다.";
        items[1].MAX_COUNT = 30;
        items[1].icon = Resources.Load<Sprite>("Images/Wood");
        //Debug 임시 아이템 데이터 끝
    }
}
