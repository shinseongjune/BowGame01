using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isMovable = true;
    public bool isAttackable = true;
    public bool isBlinkable = true;

    public bool isSkillMoving = false;
    public float skillMovingTime;
}
