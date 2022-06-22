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

    //<�ǹ� index, (������ index, ������ count)>
    public Dictionary<int, Dictionary<int, int>> costs = new();

    private void Start()
    {
        //TODO: test�ڵ� ����
        costs.Add(0, new Dictionary<int, int>());
        costs[0].Add(1, 4);
        costs[0].Add(2, 4);
        
        costs.Add(1, new Dictionary<int, int>());
        costs[1].Add(1, 2);
        //test�ڵ� ��
    }
}
