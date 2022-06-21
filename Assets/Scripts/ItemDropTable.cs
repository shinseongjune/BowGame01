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

    //<적 id, (아이템 id, 수량)>
    public Dictionary<int, (int, int)> dropTables = new();

    private void Start()
    {
        //TODO: test코드 시작
        dropTables.Add(0, (1, 10));
        dropTables.Add(0, (2, 8));
        //test코드 끝
    }
}
