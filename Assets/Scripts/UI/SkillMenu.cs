using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    

    GameObject skillMenuContentPrefab;

    Transform scrollViewContent;

    private void Start()
    {
        scrollViewContent = transform.GetChild(0).GetChild(0).GetChild(0);

        skillMenuContentPrefab = Resources.Load<GameObject>("Prefabs/SkillMenuContentPrefab");
    }
}
