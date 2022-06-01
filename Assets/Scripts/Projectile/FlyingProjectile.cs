using UnityEngine;

public class FlyingProjectile : MonoBehaviour
{
    public float speed;
    public Aggression aggression;

    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (aggression.attacker == other.gameObject)
        {
            return;
        }
        Damageable damageable = other.gameObject.transform.root.GetComponent<Damageable>();

        if (damageable != null)
        {
            aggression.target = other.gameObject;
            damageable.Damaged(aggression);
            print("test1!");
        }

        //TODO: ȭ�� �����°� ���߿� �����Ұ�. ���ص� ������.
        Destroy(gameObject);
    }
}
