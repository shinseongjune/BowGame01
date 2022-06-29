using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    //0 = basic, 1 = movement, 2 = ult
    public int current = 0;

    GameObject skillMenuContentPrefab;

    Transform scrollViewContent;

    [SerializeField] PlayerSkills playerSkills;

    private void Start()
    {
        scrollViewContent = transform.GetChild(0).GetChild(0).GetChild(0);

        skillMenuContentPrefab = Resources.Load<GameObject>("Prefabs/SkillMenuContentPrefab");
    }

    public void GetSkills()
    {
        List<int> skills = playerSkills.basicSkills;
        switch (current)
        {
            case 1:
                skills = playerSkills.movementSkills;
                break;
            case 2:
                skills = playerSkills.ults;
                break;
        }

        foreach (int skillId in skills)
        {

        }
    }
}
