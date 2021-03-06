using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public abstract bool Invoke();
}

public abstract class BasicSkill : ISkill
{
    public Dictionary<Aggression.DamageType, float> damages = new();

    public int id;

    public string name;
    public string description;

    public float coolDown;

    public GameObject afterEffect;

    public Aggression.Type type;

    public GameObject owner;
    public GameObject item;
    public Vector3 direction;

    public float reach;

    public GameObject skillPrefab;

    public Sprite skillIcon;

    //아이템 id, 소모 수량
    public Dictionary<int, int> costs = new();

    public abstract bool Invoke();
}

public class RangedSkill : BasicSkill
{
    public override bool Invoke()
    {
        //자원 소모 시작
        PlayerItemHandler itemHandler = owner.GetComponent<PlayerItemHandler>();
        if (itemHandler != null)
        {
            if (itemHandler.HasEnoughMaterialsForSkill(costs))
            {
                itemHandler.SpendMaterialsForSkill(costs);
            }
            else
            {
                //TODO: 자원 부족 문구 띄우기
                return false;
            }
        }
        //자원 소모 끝

        Aggression aggression = new(name, type, owner, null);
        GameObject projectile = Object.Instantiate(skillPrefab, owner.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(owner.transform.forward, owner.transform.up));
        FlyingProjectile flyingProjectile = projectile.GetComponent<FlyingProjectile>();
        flyingProjectile.aggression = aggression;
        flyingProjectile.restDistance = reach;
        flyingProjectile.damages = new(damages);
        if (afterEffect != null)
        {
            flyingProjectile.afterEffectPrefab = afterEffect;
        }
        return true;
    }
}

public class MeleeSkill : BasicSkill
{
    public override bool Invoke()
    {
        Aggression aggression = new(name, type, owner, null);
        //TODO: 임시 이펙트 시작
        GameObject effect = Object.Instantiate(skillPrefab, owner.transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(owner.transform.forward, owner.transform.up));
        TempMeleeAttack tempMeleeAttack = effect.GetComponent<TempMeleeAttack>();
        tempMeleeAttack.damages = new(damages);
        tempMeleeAttack.aggression = aggression;
        tempMeleeAttack.restDistance = reach;
        //임시 이펙트 끝
        return true;
    }
}

public class EmplaceSkill : BasicSkill
{
    public override bool Invoke()
    {
        Aggression aggression = new(name, type, owner, null);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 1000, layer))
        {
            if (Mathf.Approximately(Vector3.Dot(hit.normal, Vector3.up), 0))
            {
                //TODO: 적절하지 않은 위치 문구 띄우기
                return false;
            }

            if (Mathf.Approximately(owner.transform.position.y, hit.point.y))
            {
                Vector3 direction = hit.point - owner.transform.position;
                if (direction.magnitude > reach)
                {
                    targetPoint = owner.transform.position + direction.normalized * reach;
                }
                else
                {
                    targetPoint = hit.point;
                }
            }
            else
            {
                targetPoint = owner.transform.position + owner.transform.forward * reach;
            }
        }
        else
        {
            targetPoint = owner.transform.forward * reach;
        }

        bool isOnRock = false;

        RaycastHit rockCheckHit;
        if (Physics.Raycast(new Ray(owner.transform.position + new Vector3(0, 0.5f, 0), Vector3.down), out rockCheckHit, 1000, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (rockCheckHit.collider.name == "RockTile(Clone)")
            {
                isOnRock = true;
            }
        }

        layer = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("FieldResources") | 1 << LayerMask.NameToLayer("Construct") | 1 << LayerMask.NameToLayer("Installations");

        Vector3 checker = owner.transform.position + new Vector3(0, 0.51f, 0);
        float distance = 0f;

        bool isAtEndLine = false;

        bool isThereSpace = false;

        float totalDistance = (targetPoint - owner.transform.position).magnitude;
        while (distance <= totalDistance)
        {
            Collider[] hits = Physics.OverlapSphere(checker, 0.5f, layer);
            if (hits.Length == 0)
            {
                targetPoint = checker - new Vector3(0, 0.51f, 0);
                isThereSpace = true;
                if (isOnRock)
                {
                    if (Physics.Raycast(new Ray(checker, Vector3.down), out rockCheckHit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                    {
                        if (rockCheckHit.collider.name != "RockTile(Clone)")
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == 1 << LayerMask.NameToLayer("EndLine"))
                    {
                        isAtEndLine = true;
                        break;
                    }
                }
            }

            if (isAtEndLine)
            {
                break;
            }

            checker += owner.transform.forward * 0.1f;
            distance += owner.transform.forward.magnitude * 0.1f;
        }

        if (isThereSpace)
        {
            GameObject go = Object.Instantiate(skillPrefab, targetPoint, Quaternion.identity);
            Transform[] allChildren = go.GetComponentsInChildren<Transform>();
            foreach (Transform t in allChildren)
            {
                t.gameObject.tag = owner.tag;
            }

            Installation installation = go.GetComponent<Installation>();
            if (installation != null)
            {
                installation.aggression = aggression;
                installation.damages = new(damages);
            }

            //TODO: 설치 위치에 데미지 주기
            return true;
        }
        else
        {
            //TODO: 공간 부족 문구 띄우기
            return false;
        }
    }
}

public abstract class MovementSkill : ISkill
{
    public int id;
    public string name;
    public string description;
    public GameObject owner;
    public Vector3 direction;
    public float coolDown;

    public GameObject afterEffect;

    public float power;

    public float movingTime;

    public Sprite skillIcon;

    public GameObject effect;

    //TODO:테스트용. 지울것
    public Color color;
    //테스트용 끝

    public abstract bool Invoke();
}

public class DashSkill : MovementSkill
{
    public override bool Invoke()
    {
        if (effect != null)
        {
            GameObject go = Object.Instantiate(effect, owner.transform.position, Quaternion.identity);

            TrailRenderer trailRenderer = go.GetComponentInChildren<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.transform.SetParent(owner.transform);
                Object.Destroy(trailRenderer.gameObject, 0.2f);
            }
            else
            {
                Object.Destroy(go, 0.2f);
            }
        }

        Rigidbody rb = owner.GetComponent<Rigidbody>();
        rb.velocity = owner.transform.forward * power;

        //TODO:테스트용. 지울것
        owner.GetComponentInChildren<MeshRenderer>().material.color = color;
        //테스트용 끝

        return true;
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

    public override bool Invoke()
    {
        if (effect != null)
        {
            GameObject go = Object.Instantiate(effect, owner.transform.position, Quaternion.identity);

            TrailRenderer trailRenderer = go.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.transform.SetParent(owner.transform);
                Object.Destroy(trailRenderer.gameObject, 0.2f);
            }
            else
            {
                Object.Destroy(go, 0.2f);
            }
        }

        Vector3 destination = owner.transform.position + owner.transform.forward * power;

        int layer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("FieldResources") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Construct");

        if (Physics.OverlapSphere(destination + new Vector3(0, 0.51f, 0), 0.5f, layer).Length != 0)
        {
            Vector3 checker = owner.transform.position + new Vector3(0, 0.51f, 0);
            float distance = 0f;
            destination = owner.transform.position;
            
            bool isAtEndLine = false;

            while (distance <= power + 0.2f)
            {
                Collider[] hits = Physics.OverlapSphere(checker, 0.5f, layer);
                if (hits.Length == 0)
                {
                    destination = checker - new Vector3(0, 0.51f, 0);
                }
                else
                {
                    foreach (Collider h in hits)
                    {
                        if (h.CompareTag("EndLine"))
                        {
                            isAtEndLine = true;
                            break;
                        }
                    }
                }

                if (isAtEndLine)
                {
                    break;
                }

                checker += owner.transform.forward * 0.1f;
                distance += owner.transform.forward.magnitude * 0.1f;
            }
        }

        owner.transform.position = destination;

        owner.GetComponentInChildren<MeshRenderer>().material.color = color;
        
        return true;
    }
}

public abstract class Ult : ISkill
{
    public int id;
    public string name;
    public string description;

    public GameObject afterEffect;

    public Aggression.Type type;

    public Sprite skillIcon;

    public abstract bool Invoke();
}

//TODO: 궁극기는 각각 따로따로 만들어야될듯.