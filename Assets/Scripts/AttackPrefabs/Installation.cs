using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    public Aggression aggression;

    public Transform body;
    public Transform shootPoint;

    public Dictionary<Aggression.Type, float> damages = new();

    public float reach = 3.0f;

    public Transform target;

    public GameObject bulletPrefab;

    float restCoolDown = 0f;
    float coolDown = 0.8f;

    float duration = 17.0f;

    void Start()
    {
        body = transform.GetChild(0).GetChild(0);
        shootPoint = body.GetChild(0).GetChild(0);
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null)
        {
            Collider[] hits = Physics.OverlapSphere(body.position, reach);

            foreach(Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    target = hit.transform;
                    break;
                }
            }
        }

        if (target != null)
        {
            if (Vector3.Distance(target.position, body.position) > reach)
            {
                target = null;
            }
            else
            {
                Quaternion lookRotation = Quaternion.LookRotation(target.position - body.position, Vector3.up);
                body.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

                Shoot();        
            }
        }
    }

    void Shoot()
    {
        if (restCoolDown > 0)
        {
            restCoolDown -= Time.deltaTime;
            return;
        }

        Aggression newAgg = new(aggression.name, aggression.attacker, null);
        newAgg.damages = new(damages);

        GameObject go = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        go.tag = gameObject.tag;
        FlyingProjectile flyingProjectile = go.GetComponent<FlyingProjectile>();

        if (flyingProjectile != null)
        {
            flyingProjectile.aggression = newAgg;
            flyingProjectile.damages = new(damages);
        }

        restCoolDown = coolDown;
    }
}
