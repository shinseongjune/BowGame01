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

    public Aggression aggression;

    public List<GameObject> hitObjects = new();

    void Update()
    {
        if (type == Type.Explosion)
        {
            transform.localScale += new Vector3(3.7f, 3.7f, 3.7f) * Time.deltaTime;
        }
        lifeTime += Time.deltaTime;
        if (lifeTime > 0.1f)
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
                damageable.Damaged(aggression);
            }
        }
    }
}
