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

    public List<GameObject> afterEffects = new();

    private void Start()
    {
        //TODO: ���� �б�� ��ų ����
        //Debug: �׽�Ʈ �ڵ� ����
        MeleeSkill testAttack = new();
        testAttack.id = 0;
        testAttack.name = "�Ǽ� ����";
        testAttack.description = "�Ǽ� �����Դϴ�.";
        testAttack.damage = 3;
        testAttack.coolDown = 0.2f;
        testAttack.reach = 2.0f;
        testAttack.type = Aggression.Type.Attack;
        testAttack.skillPrefab = Resources.Load<GameObject>("Prefabs/TempMeleeAttack");
        testAttack.skillIcon = Resources.Load<Sprite>("Images/meleeAttack");
        defaultSkills[0] = testAttack;

        RangedSkill testBowShot = new();
        testBowShot.id = 1;
        testBowShot.name = "ȭ�� ����";
        testBowShot.description = "ȭ���� �߻��մϴ�.";
        testBowShot.damage = 10;
        testBowShot.coolDown = 0.4f;
        testBowShot.reach = 30.0f;
        testBowShot.type = Aggression.Type.Attack;
        testBowShot.skillPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/ArrowProjectile");
        testBowShot.costs.Add(3, 1);
        testBowShot.skillIcon = Resources.Load<Sprite>("Images/ArrowAttack");
        testBowShot.afterEffect = afterEffects[0];
        defaultSkills[1] = testBowShot;

        DashSkill testMovement = new();
        testMovement.id = 0;
        testMovement.name = "����";
        testMovement.description = "������ �����մϴ�.";
        testMovement.coolDown = 5.0f;
        testMovement.power = 60.0f;
        testMovement.movingTime = 0.16f;
        testMovement.color = Color.black;
        testMovement.skillIcon = Resources.Load<Sprite>("Images/DashSkill");
        testMovement.effect = Resources.Load<GameObject>("Prefabs/DashTrail");
        movementSkills[0] = testMovement;

        BlinkSkill testBlink = new();
        testBlink.id = 1;
        testBlink.name = "����";
        testBlink.description = "Ư�� ��ġ�� �����̵��մϴ�.";
        testBlink.coolDown = 7.0f;
        testBlink.power = 5.0f;
        testBlink.movingTime = 0.1f;
        testBlink.color = Color.blue;
        testBlink.skillIcon = Resources.Load<Sprite>("Images/BlinkSkill");
        testBlink.effect = Resources.Load<GameObject>("Prefabs/BlinkParticle");
        movementSkills[1] = testBlink;

        EmplaceSkill testEmplace = new();
        testEmplace.id = 0;
        testEmplace.name = "�ڵ� ��ž";
        testEmplace.description = "�ڵ� ��ž�� ��ġ�մϴ�.";
        testEmplace.coolDown = 13.0f;
        testEmplace.damage = 0;
        testEmplace.reach = 5.0f;
        testEmplace.type = Aggression.Type.Attack;
        testEmplace.skillPrefab = Resources.Load<GameObject>("Prefabs/AutoTurret");
        testEmplace.costs.Add(2, 1);
        testEmplace.costs.Add(3, 1);
        testEmplace.skillIcon = Resources.Load<Sprite>("Images/AutoTurret");
        basicSkills[0] = testEmplace;

        RangedSkill testMagicMissile = new();
        testMagicMissile.id = 1;
        testMagicMissile.name = "���� �̻���";
        testMagicMissile.description = "���� �̻����� �߻��մϴ�.";
        testMagicMissile.damage = 0;
        testMagicMissile.coolDown = 1.2f;
        testMagicMissile.reach = 30.0f;
        testMagicMissile.type = Aggression.Type.Lightning;
        testMagicMissile.skillPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MagicMissileProjectile");
        testMagicMissile.skillIcon = Resources.Load<Sprite>("Images/MagicMissile");
        testMagicMissile.afterEffect = afterEffects[1];
        basicSkills[1] = testMagicMissile;

        //Debug: �׽�Ʈ �ڵ� ��
    }
}
