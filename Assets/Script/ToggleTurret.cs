using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleTurret : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject turretPrefab;  //模型

    public bool isOn = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOn = false;
    }
}
