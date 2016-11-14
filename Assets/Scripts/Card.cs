using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public GameObject unit;
    public GameObject unitContainer;

    public void Init(string unit_name) {
        GetComponent<Animator>().enabled = false;

        for (int i=0; i<unitContainer.transform.childCount; i++) {
            Destroy(unitContainer.transform.GetChild(i).gameObject);
        }

        unit = Instantiate(Resources.Load("units/" + unit_name) as GameObject, unitContainer.transform);
        unit.transform.localPosition = Vector3.zero;
        unit.transform.localScale = new Vector3(1f, 1f, 1f);

        GetComponent<CanvasGroup>().alpha = 1;

        unit.GetComponent<Animator>().enabled = false;
    }

    public void Hide() {
        GetComponent<Animator>().enabled = true;
    }
}
