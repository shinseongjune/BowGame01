using System.Collections.Generic;
using UnityEngine;

public class FlyingProjectile : MonoBehaviour
{
    public float speed;
    public Aggression aggression;

    public float restTime = 8;
    public float restDistance = 30;

    public Dictionary<Aggression.Type, float> damages = new();

    public GameObject afterEffectPrefab;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        restTime -= Time.deltaTime;
        restDistance -= speed * Time.deltaTime;
        if (restTime <= 0 || restDistance <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(aggression.attacker.transform))
        {
            return;
        }

        if (other.gameObject.CompareTag(aggression.attacker.tag))
        {
            return;
        }

        Damageable damageable = other.gameObject.transform.root.GetComponent<Damageable>();

        if (damageable != null)
        {
            aggression.target = other.gameObject;
            damageable.Damaged(aggression, damages);
        }

        DoAfterEffect(other);

        Destroy(gameObject);
    }

    public virtual void DoAfterEffect(Collider other)
    {
        if (afterEffectPrefab != null)
        {
            GameObject go = Instantiate(afterEffectPrefab, transform.position, transform.rotation);
            AfterEffect afterEffect = go.GetComponent<AfterEffect>();
            if (afterEffect != null)
            {
                afterEffect.aggression = aggression;
                afterEffect.hitObjects.Add(other.gameObject);
            }
        }
    }
}
