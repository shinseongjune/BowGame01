using System.Collections.Generic;
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

    public Aggression.Type type;

    public GameObject owner;
    public GameObject item;
    public Vector3 direction;

    public float reach;

    public GameObject attackPrefab;

    public Sprite skillIcon;

    //������ id, �Ҹ� ����
    public Dictionary<int, int> costs = new();

    public abstract void Invoke();
}

public class RangedSkill : BasicSkill
{
    public override void Invoke()
    {
        //TODO: �ӽ� ����. owner�� �߻� ��ġ empty object ���� �ش� ��ġ���� �߻�ǰ�.
        //target ���� ���� ������ ���߿� �����ϱ�.
        //damage�� owner�� ������ �޾ƿͼ� ��� �ϴ°� �´µ�. �ϴ��� �����س��´�.
        Aggression aggression = new(name, type, damage, owner, null);
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
        Aggression aggression = new(name, type, damage, owner, null);
        //TODO: �ӽ� ����Ʈ ����
        GameObject effect = Object.Instantiate(attackPrefab, owner.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(owner.transform.forward, owner.transform.up));
        TempMeleeAttack tempMeleeAttack = effect.GetComponent<TempMeleeAttack>();
        tempMeleeAttack.aggression = aggression;
        tempMeleeAttack.restDistance = reach;
        //�ӽ� ����Ʈ ��
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
    public int id;
    public GameObject owner;
    public Vector3 direction;
    public float coolDown;

    public float power;

    public float movingTime;

    public Sprite skillIcon;

    public GameObject effect;

    //TODO:�׽�Ʈ��. �����
    public Color color;
    //�׽�Ʈ�� ��

    public abstract void Invoke();
}

public class DashSkill : MovementSkill
{
    public override void Invoke()
    {
        if (effect != null)
        {
            GameObject go = Object.Instantiate(effect, owner.transform.position, Quaternion.identity);

            TrailRenderer trailRenderer = go.GetComponentInChildren<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.transform.SetParent(owner.transform);
            }
            Object.Destroy(go, 0.2f);
        }

        Rigidbody rb = owner.GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * power;

        //TODO:�׽�Ʈ��. �����
        owner.GetComponentInChildren<MeshRenderer>().material.color = color;
        //�׽�Ʈ�� ��
    }
}

public class BlinkSkill : MovementSkill
{
    class ObstaclePoints
    {
        public Vector3 enter;
        public Vector3 exit;

        public ObstaclePoints(Vector3 enter, Vector3 exit)
        {
            this.enter = enter;
            this.exit = exit;
        }
    }

    public override void Invoke()
    {
        if (effect != null)
        {
            GameObject go = Object.Instantiate(effect, owner.transform.position, Quaternion.identity);

            TrailRenderer trailRenderer = go.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.transform.SetParent(owner.transform);
            }
            Object.Destroy(go, 0.2f);
        }

        Vector3 destination = owner.transform.position + owner.transform.forward * power;

        int layer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("FieldResources") | 1 << LayerMask.NameToLayer("Enemy");

        if (Physics.OverlapSphere(destination + new Vector3(0, 0.51f, 0), 0.5f, layer).Length != 0)
        {
            Vector3 checker = owner.transform.position + new Vector3(0, 0.51f, 0);
            float distance = 0f;
            destination = owner.transform.position;

            while (distance <= power + 0.2f)
            {
                if (Physics.OverlapSphere(checker, 0.5f, layer).Length == 0)
                {
                    destination = checker - new Vector3(0, 0.51f, 0);
                }

                checker += owner.transform.forward * 0.1f;
                distance += owner.transform.forward.magnitude * 0.1f;
            }
        }

        owner.transform.position = destination;

        owner.GetComponentInChildren<MeshRenderer>().material.color = color;

    }
}

public abstract class Ult : ISkill
{
    protected int id;

    public Sprite skillIcon;

    public abstract void Invoke();
}

//TODO: �ñر�� ���� ���ε��� �����ߵɵ�.