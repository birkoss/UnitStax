using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour {

	void Awake () {

        // Create the remaining hands
        for (int i=1; i<5; i++) {
            Instantiate(transform.GetChild(0).gameObject, transform);
        }

        // Set the correct name
        for (int i=0; i<transform.childCount; i++) {
            transform.GetChild(i).name = "card_" + i;
            GameObject card = transform.GetChild(i).gameObject;
            card.GetComponent<Animator>().enabled = false;
            card.GetComponent<Card>().Init("peon");
        }
	}

    public void Hide() {
        GetComponent<Animator>().Play("Hide");
    }

    public void Show() {
        // Verify to always have the right amount of cards
        for (int i=0; i<transform.childCount; i++) {
            GameObject card = transform.GetChild(i).gameObject;
            Debug.Log(i + " : " + card.GetComponent<CanvasGroup>().alpha);
            if (card.GetComponent<CanvasGroup>().alpha <= 0) {
                card.GetComponent<Card>().Init("peon");
            }
        }

        GetComponent<Animator>().Play("Show");
    }

}
