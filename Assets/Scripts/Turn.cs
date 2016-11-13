using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Turn : MonoBehaviour, ITurnEnded {

    public GameObject map;
    public GameObject cards;

    public GameObject attacking;

    public GameObject sprite;

    private int currentPlayer = 0;

    public delegate void CallBack();

    /*
    * Unity
    */
	void Start () {
        Begin();
	}


    /*
    * Methods
    */
    private void Begin() {
        Debug.Log("Begin: " + currentPlayer);
        map.GetComponent<Map>().EnableMovement(StartAction);
    }

    private void StartAction() {
        if (currentPlayer == 0) {
            cards.SetActive(true);
        } else {
            StartCoroutine(AI());
        }
    }

    private void End() {
        Debug.Log("End...");
        cards.SetActive(false);

        currentPlayer ^= 1;

        Begin();
    }


    /*
    * CoRoutine
    */
    private IEnumerator AI() {
        Debug.Log("AI");
        yield return new WaitForSeconds(0.5f);

        // Get all active tiles
        GameObject[] tiles = map.GetComponent<Map>().active;

        // Select an active tile
        GameObject tile = tiles[Random.Range(0, tiles.Length-1)];

        // Place a unit
        GameObject unit = Instantiate(sprite, tile.transform);
        unit.transform.localPosition = Vector3.zero;
        unit.transform.localScale = new Vector3(1f, 1f, 1f);
        unit.GetComponent<Alive>().player = 1;

        Debug.Log("Before event...");

        map.GetComponent<Map>().OnUnitPlaced(unit);
    }


    private IEnumerator ResolveMap() {

        Debug.Log("Resolve combat");

        List<Action> actions = map.GetComponent<Map>().GetActions();

        if (actions.Count>0) {
            for (int i=0; i<actions.Count; i++) {
                GameObject attacker = actions[i].unit.transform.GetChild(2).gameObject;
                GameObject defender = actions[i].target.transform.GetChild(2).gameObject;

                if (attacker.GetComponent<Alive>().isAlive) {
                    Vector3 endPosition = defender.transform.position;
                    Vector3 startPosition = attacker.transform.position;

                    // Put the attacker temporary on top
                    attacker.transform.SetParent(map.transform.parent);

                    float speed = 0.25f;
                    float timePassed = 0f;

                    // Move to the defender
                    while (timePassed < speed) {
                        attacker.transform.position = Vector3.Lerp(startPosition, endPosition, (timePassed / speed));
                        timePassed += Time.deltaTime;
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.25f);

                    GameObject animation = Instantiate(attacking, map.transform.parent);
                    animation.transform.localScale = new Vector3(1f, 1f, 1f);
                    animation.transform.position = attacker.transform.position;

                    defender.GetComponent<Alive>().TakeDamage(attacker.GetComponent<Alive>().attack);
                    if (!defender.GetComponent<Alive>().isAlive) {
                        defender.GetComponent<Image>().sprite = defender.GetComponent<Alive>().dead;
                        defender.GetComponent<Animator>().enabled = false;
                    }

                    yield return new WaitForSeconds(0.5f);

                    Destroy(animation);

                    // Move the attacker back to it's starting position
                    timePassed = 0f;
                    while (timePassed < speed) {
                        attacker.transform.position = Vector3.Lerp(endPosition, startPosition, (timePassed / speed));
                        timePassed += Time.deltaTime;
                        yield return null;
                    }

                    // Restore the attacker's position to a tile
                    attacker.transform.SetParent(actions[i].unit.transform);

                    yield return new WaitForSeconds(0.25f);

                    map.GetComponent<Map>().Clean();
                }
            }
        }

        // Resolve combat
        yield return new WaitForSeconds(0.5f);

        End();
    }

    /*
    * Events
    */
    public void OnTurnEnded() {
        StartCoroutine(ResolveMap());
    }


}

namespace UnityEngine.EventSystems {
    public interface ITurnEnded : IEventSystemHandler {
        void OnTurnEnded();
    }
}
