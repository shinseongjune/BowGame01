using UnityEngine;

public class FlyingProjectile : MonoBehaviour
{
    public float speed;
    public Aggression aggression;

    public float restTime = 8;
    public float restDistance = 30;

    public GameObject afterEffect;

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
            damageable.Damaged(aggression);
        }

        if (afterEffect != null)
        {
            DoAfterEffect();
        }

        Destroy(gameObject);
    }

    public virtual void DoAfterEffect()
    {
        GameObject go = Instantiate(afterEffect, transform.position, transform.rotation);
        go.GetComponent<AfterEffect>().aggression = aggression;
    }
}
