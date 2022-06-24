using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UltSkillSlot : MonoBehaviour, IPointerClickHandler
{
    public int? skillId;

    public float cooldown;
    public float adaptedCooldown;

    public bool isOnCooldown = false;

    [SerializeField]
    SkillDataBase skillDataBase;

    Image image;

    Image cooldownMask;

    private void Awake()
    {
        image = GetComponent<Image>();
        cooldownMask = transform.GetChild(1).GetComponent<Image>();
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

            image.sprite = skillDataBase.ults[(int)skillId].skillIcon;
        }
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);
            cooldownMask.fillAmount = cooldown / adaptedCooldown;
            if (cooldown <= 0)
            {
                isOnCooldown = false;
            }
        }
    }

    public void SetCooldown(float cooldown)
    {
        adaptedCooldown = cooldown;
        this.cooldown = adaptedCooldown;
        cooldownMask.fillAmount = 1;

        isOnCooldown = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
