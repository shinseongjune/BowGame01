using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillMenuContentPrefab : MonoBehaviour, IPointerClickHandler
{
    public int skillId;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI description;

    public SkillMenu parent;
    public Transform skillSlots;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        description = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //0 = Q basic, 1 = E basic, 2 = movement, 3 = ult
        switch (parent.current)
        {
            case 0:
                BasicSkillSlot qBasic = skillSlots.GetChild(2).GetComponent<BasicSkillSlot>();

                qBasic.SetSkill(skillId);
                break;
            case 1:
                BasicSkillSlot eBasic = skillSlots.GetChild(3).GetComponent<BasicSkillSlot>();

                eBasic.SetSkill(skillId);
                break;
            case 2:
                MovementSkillSlot movement = skillSlots.GetChild(0).GetComponent<MovementSkillSlot>();

                movement.SetSkill(skillId);
                break;
            case 3:
                UltSkillSlot ult = skillSlots.GetChild(4).GetComponent<UltSkillSlot>();

                ult.SetSkill(skillId);
                break;
        }
    }
}
