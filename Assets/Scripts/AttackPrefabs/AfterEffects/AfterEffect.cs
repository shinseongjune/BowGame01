using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffect : MonoBehaviour
{
    public enum Type
    {
        Effect,
        Explosion,
    }

    public Type type;

    public float lifeTime = 0;
    public float maxLifeTime = 1f;

    public Aggression aggression;

    public List<GameObject> hitObjects = new();

    public Dictionary<Aggression.DamageType, float> damages = new();

    void Update()
    {
        if (type == Type.Explosion)
        {
            transform.localScale += new Vector3(5f, 5f, 5f) * Time.deltaTime;
        }
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(aggression.attacker.tag))
        {
            return;
        }

        if (hitObjects.Contains(other.gameObject))
        {
            return;
        }

        if (type == Type.Explosion)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Damaged(aggression, damages);
            }
        }
    }
}
