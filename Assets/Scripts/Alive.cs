using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alive : MonoBehaviour {

    public int maxHealth;
    public int attack;
    public int player;

    public Sprite dead;

    private int health;

    public bool isAlive {
        get { return health > 0; }
    }

    public void Awake() {
        health = maxHealth;
    }

    public void TakeDamage(int damage) {
        health = Mathf.Max(0, health-damage);
    }


}
