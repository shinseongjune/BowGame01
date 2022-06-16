using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MovingItemSlotPrefab : MonoBehaviour
{
    public int itemId;
    public int count;

    public Canvas movingItemCanvas;

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(movingItemCanvas.transform as RectTransform, Input.mousePosition, movingItemCanvas.worldCamera, out pos);
        transform.position = movingItemCanvas.transform.TransformPoint(pos);
    }
}
