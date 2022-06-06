using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlHandler : MonoBehaviour
{
    Rigidbody rb;
    
    PlayerState state;
    PlayerSkillSlots slots;

    [SerializeField]
    SkillDataBase skillDataBase;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        slots = GetComponent<PlayerSkillSlots>();
    }

    void Update()
    {
        //회전 시작
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")) && !state.isSkillMoving && !state.isAttacking)
        {
            rb.MoveRotation(Quaternion.LookRotation(hit.point - transform.position, Vector3.up));
        }
        //회전 끝

        //좌클릭 시작
        if (Input.GetMouseButton(0))
        {
            if (!state.isInCombat) //전투 중이 아닐 경우
            {
                if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                {
                    //TODO: 아이템 획득 시작
                    //아이템 획득 끝
                }
                else
                {
                    if (slots.defaultCooldown <= 0)
                    {
                        DoBasicAttack();
                    }
                }
            }
            else //전투 중일 경우
            {
                if (slots.defaultCooldown <= 0)
                {
                    DoBasicAttack();
                }
            }
        }
        //좌클릭 끝

        //스페이스바 시작
        if (Input.GetKey(KeyCode.Space) && slots.movementSkillCooldown <= 0)
        {
            MovementSkill skill = skillDataBase.movementSkills[slots.movementSkill];
            if (skill is DashSkill)
            {
                if (state.isMovable)
                {
                    DoMovementSkill(skill);
                }
            }
            else if (skill is BlinkSkill)
            {
                if (state.isBlinkable)
                {
                    DoMovementSkill(skill);
                }
            }
        }
        //스페이스바 끝
    }

    void DoBasicAttack()
    {
        if (state.isAttackable)
        {
            BasicSkill skill = skillDataBase.defaultSkills[slots.defaultSkill];
            skill.owner = gameObject;
            slots.defaultCooldown = skill.coolDown;
            skill.Invoke();
        }
    }

    void DoMovementSkill(MovementSkill skill)
    {
        skill.owner = gameObject;
        skill.direction = gameObject.transform.forward;
        state.isSkillMoving = true;
        state.skillMovingTime = skill.movingTime;
        slots.movementSkillCooldown = skill.coolDown;
        skill.Invoke();
    }
}
