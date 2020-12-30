using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class box_julingqu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
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
