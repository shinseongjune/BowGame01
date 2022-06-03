using UnityEngine;

public class PlayerSkillSlots : MonoBehaviour
{
    PlayerState state;

    public int defaultSkill;
    public int[] basicSkills = new int[2];
    public int movementSkill;
    public int ult;

    public float defaultCooldown = 0;
    public float basicSkillCooldown1 = 0;
    public float basicSkillCooldown2 = 0;
    public float movementSkillCooldown = 0;
    public float ultCooldown = 0;

    [SerializeField]
    SkillDataBase skillDataBase;

    private void Start()
    {
        state = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && defaultCooldown <= 0)
        {
            if (state.isAttackable)
            {
                BasicSkill skill = skillDataBase.defaultSkills[defaultSkill];
                skill.owner = gameObject;
                defaultCooldown = skill.coolDown;
                skill.Invoke();
            }
        }

        if (Input.GetKey(KeyCode.Space) && movementSkillCooldown <= 0)
        {
            MovementSkill skill = skillDataBase.movementSkills[movementSkill];
            if (skill is DashSkill)
            {
                if (state.isMovable)
                {
                    skill.owner = gameObject;
                    skill.direction = gameObject.transform.forward;
                    state.isSkillMoving = true;
                    state.skillMovingTime = skill.movingTime;
                    movementSkillCooldown = skill.coolDown;
                    skill.Invoke();
                }
            }
            else if (skill is BlinkSkill)
            {
                if (state.isBlinkable)
                {
                    skill.owner = gameObject;
                    skill.direction = gameObject.transform.forward;
                    state.isSkillMoving = true;
                    state.skillMovingTime = skill.movingTime;
                    movementSkillCooldown = skill.coolDown;
                    skill.Invoke();
                }
            }
        }

        defaultCooldown = Mathf.Clamp(defaultCooldown - Time.deltaTime, 0, float.MaxValue);
        basicSkillCooldown1 = Mathf.Clamp(basicSkillCooldown1 - Time.deltaTime, 0, float.MaxValue);
        basicSkillCooldown2 = Mathf.Clamp(basicSkillCooldown2 - Time.deltaTime, 0, float.MaxValue);
        movementSkillCooldown = Mathf.Clamp(movementSkillCooldown - Time.deltaTime, 0, float.MaxValue);
        ultCooldown = Mathf.Clamp(ultCooldown - Time.deltaTime, 0, float.MaxValue);
    }
}
