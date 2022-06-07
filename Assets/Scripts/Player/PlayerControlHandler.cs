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

    [SerializeField] Canvas basicModeCanvas;
    [SerializeField] Canvas buildingModeCanvas;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        slots = GetComponent<PlayerSkillSlots>();
    }

    void Update()
    {
        //ȸ�� ����
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")) && !state.isSkillMoving && !state.isAttacking)
        {
            rb.MoveRotation(Quaternion.LookRotation(hit.point - transform.position, Vector3.up));
        }
        //ȸ�� ��

        if (state.isBuilding) //�Ǽ� ���
        {
            if (Input.GetKeyDown(KeyCode.B)) //�Ǽ� ��� ���
            {
                state.isBuilding = false;
                buildingModeCanvas.gameObject.SetActive(false);
                basicModeCanvas.gameObject.SetActive(true);
            }

            //TODO: ���๰ ����, ���콺 ���� ���๰ �پ�ٴϱ�, ���� ���� ���� ǥ��

            //TODO: ��Ŭ�� ����
            if (Input.GetMouseButton(0))
            {

            }
            //��Ŭ�� ��

            //�����̽��� ����
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
            //�����̽��� ��
        }
        else //�Ϲ� ���
        {
            if (Input.GetKeyDown(KeyCode.B)) //�Ǽ� ��� ����
            {
                if (!state.isInCombat)
                {
                    state.isBuilding = true;
                    basicModeCanvas.gameObject.SetActive(false);
                    buildingModeCanvas.gameObject.SetActive(true);
                }
            }

            //��Ŭ�� ����
            if (Input.GetMouseButton(0))
            {
                if (!state.isInCombat) //���� ���� �ƴ� ���
                {
                    if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                    {
                        //TODO: ������ ȹ�� ����
                        //������ ȹ�� ��
                    }
                    else
                    {
                        if (slots.defaultCooldown <= 0)
                        {
                            DoBasicAttack();
                        }
                    }
                }
                else //���� ���� ���
                {
                    if (slots.defaultCooldown <= 0)
                    {
                        DoBasicAttack();
                    }
                }
            }
            //��Ŭ�� ��

            //�����̽��� ����
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
            //�����̽��� ��
        }
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
