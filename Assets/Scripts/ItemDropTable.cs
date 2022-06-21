using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{
    //Singleton start
    private static ItemDropTable instance = null;

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

    //<�� id, (������ id, ����)>
    public Dictionary<int, (int, int)> dropTables = new();

    private void Start()
    {
        //TODO: test�ڵ� ����
        dropTables.Add(0, (1, 10));
        dropTables.Add(0, (2, 8));
        //test�ڵ� ��
    }
}
