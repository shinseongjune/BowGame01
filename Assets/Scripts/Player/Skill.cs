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
//TODO: item 클래스 만들기. projectile이나 effect 등 멤버 만들것. 그후 invoke 구현하면 될듯.
//player inventory, projectile 클래스 먼저 만들어야할듯.
//stat도 만들어야할듯.

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