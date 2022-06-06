using UnityEngine;

public class PlayerSkillSlots : MonoBehaviour
{
    public int defaultSkill;
    public int[] basicSkills = new int[2];
    public int movementSkill;
    public int ult;

    public float defaultCooldown = 0;
    public float basicSkillCooldown1 = 0;
    public float basicSkillCooldown2 = 0;
    public float movementSkillCooldown = 0;
    public float ultCooldown = 0;

    private void Update()
    {
        defaultCooldown = Mathf.Clamp(defaultCooldown - Time.deltaTime, 0, float.MaxValue);
        basicSkillCooldown1 = Mathf.Clamp(basicSkillCooldown1 - Time.deltaTime, 0, float.MaxValue);
        basicSkillCooldown2 = Mathf.Clamp(basicSkillCooldown2 - Time.deltaTime, 0, float.MaxValue);
        movementSkillCooldown = Mathf.Clamp(movementSkillCooldown - Time.deltaTime, 0, float.MaxValue);
        ultCooldown = Mathf.Clamp(ultCooldown - Time.deltaTime, 0, float.MaxValue);
    }
}
