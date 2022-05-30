using UnityEngine;

public interface ISkill
{
    public abstract void Invoke();
}

public abstract class BasicSkill : ISkill
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

public abstract class MovementSkill : ISkill
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

public abstract class Ult : ISkill
{
    public abstract void Invoke();
}

//TODO: 궁극기는 각각 따로따로 만들어야될듯.