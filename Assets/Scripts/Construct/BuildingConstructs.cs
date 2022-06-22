using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstructs : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public bool isSnapped = false;
    public bool isConstructable = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material.color = Color.red;
        isConstructable = false;
    }

    private void OnTriggerStay(Collider other)
    {
        meshRenderer.material.color = Color.red;
        isConstructable = false;
    }

    private void OnTriggerExit(Collider other)
    {
        meshRenderer.material.color = Color.cyan;
        isConstructable = true;
    }
}
