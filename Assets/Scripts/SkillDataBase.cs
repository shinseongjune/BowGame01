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
        testAttack.name = "맨손 공격";
        testAttack.damage = 3;
        testAttack.coolDown = 0.2f;
        testAttack.reach = 1f;
        defaultSkills[0] = testAttack;
        RangedSkill testBowShot = new();
        testBowShot.name = "화살 공격";
        testBowShot.damage = 10;
        testBowShot.coolDown = 0.4f;
        testBowShot.reach = 30.0f;
        testBowShot.attackPrefab = Resources.Load<GameObject>("Prefabs/ArrowProjectile");
        defaultSkills[1] = testBowShot;
        //Debug: 테스트 코드 끝
    }
}
