using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed;

    public ProjectileMovement(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
