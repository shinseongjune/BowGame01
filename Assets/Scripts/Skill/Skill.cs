using UnityEngine;

public interface ISkill
{
    public abstract void Invoke();
}

public abstract class BasicSkill : ISkill
{
    public int id;

    public string name;

    public float damage;

    public float coolDown;

    public GameObject owner;
    public GameObject item;
    public Vector3 direction;

    public float reach;

    public GameObject attackPrefab;

    public abstract void Invoke();
}

public class RangedSkill : BasicSkill
{
    public override void Invoke()
    {
        //TODO: �ӽ� ����. owner�� �߻� ��ġ empty object ���� �ش� ��ġ���� �߻�ǰ�.
        //target ���� ���� ������ ���߿� �����ϱ�.
        //damage�� owner�� ������ �޾ƿͼ� ��� �ϴ°� �´µ�. �ϴ��� �����س��´�.
        Aggression aggression = new(name, Aggression.Type.Attack, damage, owner, null);
        GameObject projectile = Object.Instantiate(attackPrefab, owner.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(owner.transform.forward, owner.transform.up));
        FlyingProjectile flyingProjectile = projectile.GetComponent<FlyingProjectile>();
        flyingProjectile.aggression = aggression;
        flyingProjectile.restDistance = reach;
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