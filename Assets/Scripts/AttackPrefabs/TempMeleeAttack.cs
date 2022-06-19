using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMeleeAttack : MonoBehaviour
{
    public float speed;
    public Aggression aggression;

    public float restTime = 8;
    public float restDistance = 30;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(aggression.attacker.transform))
        {
            return;
        }
        Damageable damageable = other.gameObject.transform.root.GetComponent<Damageable>();

        if (damageable != null)
        {
            aggression.target = other.gameObject;
            damageable.Damaged(aggression);
        }

        PlayerItemHandler itemHandler = aggression.attacker.GetComponent<PlayerItemHandler>();

        if (itemHandler != null)
        {
            FieldResource fieldResource = other.gameObject.transform.root.GetComponent<FieldResource>();

            if (fieldResource != null)
            {
                if (fieldResource.rest < aggression.damage)
                {
                    itemHandler.GetItem(fieldResource.itemId, fieldResource.rest);
                    fieldResource.rest = 0;
                    Destroy(fieldResource.gameObject);
                }
                else
                {
                    itemHandler.GetItem(fieldResource.itemId, (int)aggression.damage);
                    fieldResource.rest -= (int)aggression.damage;
                }
            }
        }
    }
}