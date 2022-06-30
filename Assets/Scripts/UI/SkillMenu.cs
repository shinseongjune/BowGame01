using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    //0 = Q basic, 1 = E basic, 2 = movement, 3 = ult
    public int current = 0;

    GameObject skillMenuContentPrefab;

    Transform scrollViewContent;

    [SerializeField] PlayerSkills playerSkills;
    [SerializeField] SkillDataBase skillDataBase;
    [SerializeField] Transform skillSlots;

    private void Start()
    {
        scrollViewContent = transform.GetChild(0).GetChild(0).GetChild(0);

        skillMenuContentPrefab = Resources.Load<GameObject>("Prefabs/SkillMenuContentPrefab");
    }

    public void SetCurrent(int i)
    {
        current = i;
        GetSkills();
    }

    public void GetSkills()
    {
        foreach (Transform t in scrollViewContent.transform)
        {
            if (t != scrollViewContent.transform)
            {
                Destroy(t.gameObject);
            }
        }

        List<int> learnedSkills = playerSkills.basicSkills;
        switch (current)
        {
            case 2:
                learnedSkills = playerSkills.movementSkills;
                break;
            case 3:
                learnedSkills = playerSkills.ults;
                break;
        }

        switch (current)
        {
            case 0:
            case 1:
                Dictionary<int, BasicSkill> basics = skillDataBase.basicSkills;

                foreach (int skillId in learnedSkills)
                {
                    GameObject go = Instantiate(skillMenuContentPrefab, scrollViewContent);
                    SkillMenuContentPrefab content = go.GetComponent<SkillMenuContentPrefab>();

                    content.skillId = skillId;
                    content.icon.sprite = basics[skillId].skillIcon;
                    content.nameText.text = basics[skillId].name;
                    content.description.text = basics[skillId].description;

                    content.parent = this;
                    content.skillSlots = skillSlots;
                }
                break;
            case 2:
                Dictionary<int, MovementSkill> movements = skillDataBase.movementSkills;

                foreach (int skillId in learnedSkills)
                {
                    GameObject go = Instantiate(skillMenuContentPrefab, scrollViewContent);
                    SkillMenuContentPrefab content = go.GetComponent<SkillMenuContentPrefab>();

                    content.skillId = skillId;
                    content.icon.sprite = movements[skillId].skillIcon;
                    content.nameText.text = movements[skillId].name;
                    content.description.text = movements[skillId].description;

                    content.parent = this;
                    content.skillSlots = skillSlots;
                }
                break;
            case 3:
                Dictionary<int, Ult> ults = skillDataBase.ults;

                foreach (int skillId in learnedSkills)
                {
                    GameObject go = Instantiate(skillMenuContentPrefab, scrollViewContent);
                    SkillMenuContentPrefab content = go.GetComponent<SkillMenuContentPrefab>();

                    content.skillId = skillId;
                    content.icon.sprite = ults[skillId].skillIcon;
                    content.nameText.text = ults[skillId].name;
                    content.description.text = ults[skillId].description;

                    content.parent = this;
                    content.skillSlots = skillSlots;
                }
                break;
        }
    }
}
