using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase : MonoBehaviour
{
    //Singleton start
    private static SkillDataBase instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Singleton end

    public Dictionary<int, BasicSkill> defaultSkills = new();
    public Dictionary<int, BasicSkill> basicSkills = new();
    public Dictionary<int, MovementSkill> movementSkills = new();
    public Dictionary<int, Ult> ults = new();

    private void Start()
    {
        //TODO: 파일 읽기로 스킬 관리
        //Debug: 테스트 코드 시작
        MeleeSkill testAttack = new();
        testAttack.id = 0;
        testAttack.name = "맨손 공격";
        testAttack.damage = 3;
        testAttack.coolDown = 0.2f;
        testAttack.reach = 2.0f;
        testAttack.type = Aggression.Type.Attack;
        testAttack.attackPrefab = Resources.Load<GameObject>("Prefabs/TempMeleeAttack");
        testAttack.skillIcon = Resources.Load<Sprite>("Images/meleeAttack");
        defaultSkills[0] = testAttack;
        RangedSkill testBowShot = new();
        testBowShot.id = 1;
        testBowShot.name = "화살 공격";
        testBowShot.damage = 10;
        testBowShot.coolDown = 0.4f;
        testBowShot.reach = 30.0f;
        testBowShot.type = Aggression.Type.Attack;
        testBowShot.attackPrefab = Resources.Load<GameObject>("Prefabs/ArrowProjectile");
        testBowShot.costs.Add(3, 1);
        testBowShot.skillIcon = Resources.Load<Sprite>("Images/ArrowAttack");
        defaultSkills[1] = testBowShot;
        DashSkill testMovement = new();
        testMovement.id = 0;
        testMovement.coolDown = 5.0f;
        testMovement.power = 60.0f;
        testMovement.movingTime = 0.16f;
        testMovement.color = Color.black;
        testMovement.skillIcon = Resources.Load<Sprite>("Images/DashSkill");
        movementSkills[0] = testMovement;
        //Debug: 테스트 코드 끝
    }
}
