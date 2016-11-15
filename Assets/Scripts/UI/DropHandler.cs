using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler {

    public GameObject item {
        get {
            if (transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Tile tile = GetComponent<Tile>();
        if (tile == null || (tile.isEmpty && tile.isEnable) ) {
            GameObject card = DragHandler.itemBeginDragged.transform.parent.parent.gameObject;
            card.GetComponent<Card>().Hide();

            DragHandler.itemBeginDragged.transform.SetParent(transform);
            DragHandler.itemBeginDragged.transform.localPosition = new Vector3(0f, 0f, 0f);
            DragHandler.itemBeginDragged.GetComponent<Unit>().player = 0;
            DragHandler.itemBeginDragged.GetComponent<Animator>().enabled = true;

            DragHandler.itemBeginDragged.GetComponent<DragHandler>().isDraggable = false;

            ExecuteEvents.ExecuteHierarchy<ITurnEnded>(gameObject, null, (x,y) => x.OnTurnEnded(DragHandler.itemBeginDragged));
            ExecuteEvents.ExecuteHierarchy<IUnitPlaced>(gameObject, null, (x,y) => x.OnUnitPlaced(DragHandler.itemBeginDragged));
        }
    }
}
