using UnityEngine;

public class PlayerSkillSlots : MonoBehaviour
{
    public int defaultSkill;
    public int[] basicSkills = new int[2];
    public int movementSkill;
    public int ult;

    [SerializeField]
    SkillDataBase skillDataBase;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skillDataBase.defaultSkills[defaultSkill].Invoke();
        }
    }
}
