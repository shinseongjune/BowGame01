using UnityEngine;

public interface ISkill
{
    public abstract void Invoke();
}

public abstract class BasicSkill : ISkill
{
    protected int id;
    protected GameObject owner;
    protected GameObject item;
    protected Vector3 direction;

    public abstract void Invoke();
}

public class RangedSkill : BasicSkill
{
    public override void Invoke()
    {
        Debug.Log("발사!");
    }
}

public class MeleeSkill : BasicSkill
{
    public override void Invoke()
    {
        Debug.Log("근접!");
    }
}

public class SplashSkill : BasicSkill
{
    public override void Invoke()
    {
        Debug.Log("광역!");
    }
}

public abstract class MovementSkill : ISkill
{
    protected int id;
    protected GameObject owner;
    protected Vector3 direction;

    public abstract void Invoke();
}

public class DashSkill : MovementSkill
{
    public override void Invoke()
    {
        Debug.Log("돌진!");
    }
}

public class BlinkSkill : MovementSkill
{
    public override void Invoke()
    {
        Debug.Log("점멸!");
    }
}

public abstract class Ult : ISkill
{
    protected int id;
    public abstract void Invoke();
}

//TODO: 궁극기는 각각 따로따로 만들어야될듯.