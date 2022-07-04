using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailsContentPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;

    Transform detailsDescriptionWindow;

    private void Start()
    {
        detailsDescriptionWindow = transform.root.GetChild(2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailsDescriptionWindow.gameObject.SetActive(true);
        detailsDescriptionWindow.GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailsDescriptionWindow.gameObject.SetActive(false);
    }
}
