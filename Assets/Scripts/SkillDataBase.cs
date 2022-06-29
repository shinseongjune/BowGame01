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
        testAttack.skillPrefab = Resources.Load<GameObject>("Prefabs/TempMeleeAttack");
        testAttack.skillIcon = Resources.Load<Sprite>("Images/meleeAttack");
        defaultSkills[0] = testAttack;

        RangedSkill testBowShot = new();
        testBowShot.id = 1;
        testBowShot.name = "화살 공격";
        testBowShot.damage = 10;
        testBowShot.coolDown = 0.4f;
        testBowShot.reach = 30.0f;
        testBowShot.type = Aggression.Type.Attack;
        testBowShot.skillPrefab = Resources.Load<GameObject>("Prefabs/ArrowProjectile");
        testBowShot.costs.Add(3, 1);
        testBowShot.skillIcon = Resources.Load<Sprite>("Images/ArrowAttack");
        defaultSkills[1] = testBowShot;

        DashSkill testMovement = new();
        testMovement.id = 0;
        testMovement.name = "돌진";
        testMovement.coolDown = 5.0f;
        testMovement.power = 60.0f;
        testMovement.movingTime = 0.16f;
        testMovement.color = Color.black;
        testMovement.skillIcon = Resources.Load<Sprite>("Images/DashSkill");
        testMovement.effect = Resources.Load<GameObject>("Prefabs/DashTrail");
        movementSkills[0] = testMovement;

        BlinkSkill testBlink = new();
        testBlink.id = 1;
        testBlink.name = "점멸";
        testBlink.coolDown = 7.0f;
        testBlink.power = 5.0f;
        testBlink.movingTime = 0.1f;
        testBlink.color = Color.blue;
        testBlink.skillIcon = Resources.Load<Sprite>("Images/BlinkSkill");
        testBlink.effect = Resources.Load<GameObject>("Prefabs/BlinkParticle");
        movementSkills[1] = testBlink;

        EmplaceSkill testEmplace = new();
        testEmplace.id = 0;
        testEmplace.name = "자동 포탑";
        testEmplace.coolDown = 13.0f;
        testEmplace.damage = 0;
        testEmplace.reach = 5.0f;
        testEmplace.type = Aggression.Type.Attack;
        testEmplace.skillPrefab = Resources.Load<GameObject>("Prefabs/AutoTurret");
        testEmplace.costs.Add(2, 1);
        testEmplace.costs.Add(3, 1);
        testEmplace.skillIcon = Resources.Load<Sprite>("Images/AutoTurret");
        basicSkills[0] = testEmplace;
        //Debug: 테스트 코드 끝
    }
}
