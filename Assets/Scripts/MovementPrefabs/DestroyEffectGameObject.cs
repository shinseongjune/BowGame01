using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffectGameObject : MonoBehaviour
{
    public void DestroyEffect(float duration)
    {
        Destroy(gameObject, duration);
    }
}
