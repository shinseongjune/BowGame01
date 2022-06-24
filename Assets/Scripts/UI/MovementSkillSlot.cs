using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementSkillSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotId;
    public int? skillId;

    [SerializeField]
    SkillDataBase skillDataBase;

    public GameObject player;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSkill(int? skillId)
    {
        if (skillId == null)
        {
            this.skillId = null;

            image.sprite = null;
        }
        else
        {
            this.skillId = skillId;

            image.sprite = skillDataBase.movementSkills[(int)skillId].skillIcon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
