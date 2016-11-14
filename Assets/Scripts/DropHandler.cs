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
        Debug.Log("OnDrop");

        Tile tile = GetComponent<Tile>();
        if (tile == null || (tile.isEmpty && tile.isActive) ) {
            GameObject card = DragHandler.itemBeginDragged.transform.parent.parent.gameObject;
            card.GetComponent<Card>().Hide();

            DragHandler.itemBeginDragged.transform.SetParent(transform);
            DragHandler.itemBeginDragged.transform.localPosition = new Vector3(0f, 0f, 0f);
            DragHandler.itemBeginDragged.GetComponent<Alive>().player = 0;
            DragHandler.itemBeginDragged.GetComponent<Animator>().enabled = true;

            ExecuteEvents.ExecuteHierarchy<ITurnEnded>(gameObject, null, (x,y) => x.OnTurnEnded(DragHandler.itemBeginDragged));
            // ExecuteEvents.ExecuteHierarchy<IUnitPlaced>(gameObject, null, (x,y) => x.OnUnitPlaced(DragHandler.itemBeginDragged));
        }
    }
}
