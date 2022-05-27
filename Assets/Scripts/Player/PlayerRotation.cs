using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
        {
            body.MoveRotation(Quaternion.LookRotation(hit.point - transform.position, Vector3.up));
        }
    }
}
