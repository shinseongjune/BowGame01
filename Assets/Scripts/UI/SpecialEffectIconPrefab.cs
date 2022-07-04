using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpecialEffectIconPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Transform descriptionWindow;

    public string description;

    void Start()
    {
        descriptionWindow = transform.root.GetChild(0).GetChild(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.gameObject.SetActive(true);
        descriptionWindow.GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.gameObject.SetActive(false);
    }
}
