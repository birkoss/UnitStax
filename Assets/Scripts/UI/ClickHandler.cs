using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IPointerUpHandler {

    private bool isDragged;

    /*
    * Events
    */

    public void OnPointerDown(PointerEventData eventData) {
        isDragged = false;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isDragged = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (!isDragged) {
            Debug.Log("clicked on " + gameObject);
        }
    }
}
