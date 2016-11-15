using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeginDragged;

    private Vector3 startPosition;
    private Transform startParent;

    public bool isDraggable {
        get { return enabled; }
        set { enabled = value; }
    }

    /*
    * Unity
    */
    public void Awake() {
        isDraggable = false;
    }

    /*
    * Events
    */
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
