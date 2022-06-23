using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughMaterialsTextUIPrefab : MonoBehaviour
{
    const float SPEED = 19f;
    RectTransform rect;

    void Start()
    {
        Destroy(gameObject, 0.8f);
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.position += Vector3.up * SPEED * Time.deltaTime;
    }
}
