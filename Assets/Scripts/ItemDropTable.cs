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
    public Dictionary<int, List<int>> dropTables = new();

    private void Start()
    {
        //TODO: test�ڵ� ����
        dropTables.Add(1, new());
        dropTables[1].Add(1);
        dropTables[1].Add(2);
        //test�ڵ� ��
    }
}
