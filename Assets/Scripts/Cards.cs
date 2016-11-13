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
            transform.GetChild(i).gameObject.GetComponent<Card>().unit.GetComponent<Animator>().enabled = false;
            transform.GetChild(i).gameObject.GetComponent<Animator>().enabled = false;
        }
	}

}
