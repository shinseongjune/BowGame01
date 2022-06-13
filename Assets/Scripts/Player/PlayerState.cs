using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isMovable = true;       //이동, 대쉬 가능
    public bool isAttackable = true;    //공격 가능
    public bool isBlinkable = true;     //점멸 가능

    public bool isInCombat = false;     //전투 중

    public bool isAttacking = false;    //공격 중
    public float attackTime;            //공격 시간

    public bool isSkillMoving = false;  //이동 스킬 사용 중
    public float skillMovingTime;       //이동 스킬 지속 시간

    public bool isBuilding = false;     //건설 모드
    public int buildingFloor = 0;
}
