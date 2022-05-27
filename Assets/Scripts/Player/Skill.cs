using UnityEngine;

public interface Skill
{
    public abstract void Invoke();
}

public abstract class BasicSkill : Skill
{
    protected GameObject owner;
    protected GameObject item;
    protected Vector3 direction;

    public abstract void Invoke();
}

public class RangedSkill : BasicSkill
{
    public override void Invoke()
    {

    }
}

public class MeleeSkill : BasicSkill
{
    public override void Invoke()
    {

    }
}

public class SplashSkill : BasicSkill
{
    public override void Invoke()
    {

    }
}
//TODO: item Ŭ���� �����. projectile�̳� effect �� ��� �����. ���� invoke �����ϸ� �ɵ�.
//player inventory, projectile Ŭ���� ���� �������ҵ�.
//stat�� �������ҵ�.

public abstract class MovementSkill : Skill
{
    protected GameObject owner;
    protected Vector3 direction;

    public abstract void Invoke();
}

public class DashSkill : MovementSkill
{
    public override void Invoke()
    {
        throw new System.NotImplementedException();
    }
}

public class BlinkSkill : MovementSkill
{
    public override void Invoke()
    {
        throw new System.NotImplementedException();
    }
}