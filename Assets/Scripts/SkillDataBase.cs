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
        //TODO: ���� �б�� ��ų ����
        //Debug: �׽�Ʈ �ڵ� ����
        MeleeSkill testAttack = new();
        defaultSkills[0] = testAttack;
        RangedSkill testBowShot = new();
        defaultSkills[1] = testBowShot;
        //Debug: �׽�Ʈ �ڵ� ��
    }
}
