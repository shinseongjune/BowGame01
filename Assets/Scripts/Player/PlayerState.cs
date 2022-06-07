using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isMovable = true;       //�̵�, �뽬 ����
    public bool isAttackable = true;    //���� ����
    public bool isBlinkable = true;     //���� ����

    public bool isInCombat = false;     //���� ��

    public bool isAttacking = false;    //���� ��
    public float attackTime;            //���� �ð�

    public bool isSkillMoving = false;  //�̵� ��ų ��� ��
    public float skillMovingTime;       //�̵� ��ų ���� �ð�

    public bool isBuilding = false;     //�Ǽ� ���

    private void Update()
    {
        if (isInCombat)
        {
            isBuilding = false;
        }
    }
}
