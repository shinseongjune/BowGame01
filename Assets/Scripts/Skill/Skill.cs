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
        Debug.Log("�߻�!");
    }
}

public class MeleeSkill : BasicSkill
{
    public override void Invoke()
    {
        Debug.Log("����!");
    }
}

public class SplashSkill : BasicSkill
{
    public override void Invoke()
    {
        Debug.Log("����!");
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
        Debug.Log("����!");
    }
}

public class BlinkSkill : MovementSkill
{
    public override void Invoke()
    {
        Debug.Log("����!");
    }
}

public abstract class Ult : ISkill
{
    protected int id;
    public abstract void Invoke();
}

//TODO: �ñر�� ���� ���ε��� �����ߵɵ�.