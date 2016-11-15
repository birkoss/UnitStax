using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeginDragged;

    private Vector3 startPosition;
    private Transform startParent;

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnBeginDrag(PointerEventData eventData) {
        itemBeginDragged = gameObject;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemBeginDragged = null;

        if (transform.parent == startParent) {
             transform.position = startPosition;
         }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
