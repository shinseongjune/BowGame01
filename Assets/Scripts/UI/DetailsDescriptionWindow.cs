using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsDescriptionWindow : MonoBehaviour
{
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float rectWidth = (rect.anchorMax.x - rect.anchorMin.x) * Screen.width;
        float rectHeight = (rect.anchorMax.y - rect.anchorMin.y) * Screen.height;
        transform.position = Input.mousePosition + new Vector3(rectWidth + 5, -rectHeight - 5, 0);
    }

    private void OnEnable()
    {
        float rectWidth = (rect.anchorMax.x - rect.anchorMin.x) * Screen.width;
        float rectHeight = (rect.anchorMax.y - rect.anchorMin.y) * Screen.height;
        transform.position = Input.mousePosition + new Vector3(rectWidth + 5, -rectHeight , 0);
    }
}
