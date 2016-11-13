using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    private GameObject floor;
    public GameObject active;
    private Vector2 tilePosition;

    public bool isEmpty {
        get { return (transform.childCount == 2); }
    }

    public bool isActive {
        get { return active.activeSelf; }
        set { active.SetActive(value); }
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
        active = transform.GetChild(1).gameObject;
    }

}
