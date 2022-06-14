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
        //TODO: ���� �б�� �ؾ��� �� ����. ��ųʸ��� �ٲܰ�.
        //Debug: �ӽ� ������ ������
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item();
            items[i].id = i;
        }
        items[0].itemName = "�ܱ�";
        items[0].description = "������ ���� �ܱ��̴�.";
        items[0].MAX_COUNT = 1;
        items[0].icon = Resources.Load<Sprite>("Images/ShortBow");
        items[0].defaultSkill = 1;

        items[1].itemName = "����";
        items[1].description = "�����.";
        items[1].MAX_COUNT = 30;
        items[1].icon = Resources.Load<Sprite>("Images/Wood");
        //Debug �ӽ� ������ ������ ��
    }
}
