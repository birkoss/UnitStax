using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    public GameObject active;
    public GameObject unitGameObject;

    private GameObject floor;
    private Vector2 tilePosition;

    public GameObject unit {
        get { return unitGameObject.transform.GetChild(0).gameObject; }
    }

    public bool isEmpty {
        get { return (unitGameObject.transform.childCount == 0); }
    }

    public bool isEnable {
        get { return active.GetComponent<Animator>().GetBool("isEnable"); }
        set { active.GetComponent<Animator>().SetBool("isEnable", true); }
    }

    public Vector2 position {
        get { return tilePosition; }
        set { tilePosition = value; }
    }

    /*
    * Unity
    */
    public void Awake() {
        floor = transform.GetChild(0).gameObject;


    }

}
