using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDataBase : MonoBehaviour
{
    //Singleton start
    private static BuildingDataBase instance = null;

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

    public GameObject[] constructsPrefabs;

    //<건물 index, (아이템 index, 아이템 count)>
    public Dictionary<int, Dictionary<int, int>> costs = new();

    private void Start()
    {
        //TODO: test코드 시작
        costs.Add(0, new Dictionary<int, int>());
        costs[0].Add(1, 4);
        costs[0].Add(2, 4);
        
        costs.Add(1, new Dictionary<int, int>());
        costs[1].Add(1, 2);
        //test코드 끝
    }
}
