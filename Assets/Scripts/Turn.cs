using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Turn : MonoBehaviour, ITurnEnded {

    public GameObject map;
    public GameObject cards;

    public GameObject attacking;

    private int currentPlayer = 0;

    public delegate void Callback();

    /*
    * Unity
    */
	void Start () {
        StartCoroutine(ActivateMap(Begin));
	}


    /*
    * Methods
    */
    private void Begin() {
        StartAction();
    }

    private void StartAction() {
        if (currentPlayer == 0) {
            cards.GetComponent<Cards>().Show();
        } else {
            StartCoroutine(AI());
        }
    }

    private void End() {
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
        GameObject[] tiles = map.GetComponent<Map>().available;

        // Select an active tile
        GameObject tile = tiles[Random.Range(0, tiles.Length-1)];

        // Place a unit
        GameObject unit = Instantiate(PrefabManager.Instance.Get("skeleton"), tile.transform);
        unit.transform.localPosition = Vector3.zero;
        unit.transform.localScale = new Vector3(1f, 1f, 1f);
        unit.GetComponent<Unit>().player = 1;

        map.GetComponent<Map>().OnUnitPlaced(unit);
        OnTurnEnded(unit);
    }


    private IEnumerator ResolveMap() {
        List<Action> actions = map.GetComponent<Map>().GetActions();

        if (actions.Count>0) {
            for (int i=0; i<actions.Count; i++) {
                GameObject attacker = actions[i].unit.transform.GetChild(2).gameObject;
                GameObject defender = actions[i].target.transform.GetChild(2).gameObject;

                if (attacker.GetComponent<Unit>().isAlive && defender.GetComponent<Unit>().isAlive) {
                    Vector3 endPosition = defender.transform.position;
                    Vector3 startPosition = attacker.transform.position;

                    // Put the attacker temporary on top
                    attacker.transform.SetParent(map.transform.parent);

                    // Face correctly to the defender
                    if (endPosition.x > startPosition.x) {
                        attacker.transform.localScale = new Vector3(-1f, 1f, 1f);
                    }

                    float speed = 0.15f;
                    float timePassed = 0f;

                    // Move to the defender
                    while (timePassed < speed) {
                        attacker.transform.position = Vector3.Lerp(startPosition, endPosition, (timePassed / speed));
                        timePassed += Time.deltaTime;
                        yield return null;
                    }

                    // Create attacking animation
                    GameObject animation = Instantiate(attacking, map.transform.parent);
                    animation.transform.localScale = new Vector3(1f, 1f, 1f);
                    animation.transform.position = attacker.transform.position;

                    // Apply damage
                    defender.GetComponent<Unit>().TakeDamage(attacker.GetComponent<Unit>().attack);
                    if (!defender.GetComponent<Unit>().isAlive) {
                        defender.GetComponent<Image>().sprite = defender.GetComponent<Unit>().dead;
                        defender.GetComponent<Animator>().enabled = false;
                    }

                    yield return new WaitForSeconds(0.25f);

                    Destroy(animation);

                    // Face back before returning to our tile
                    if (endPosition.x > startPosition.x) {
                        attacker.transform.localScale = new Vector3(1f, 1f, 1f);
                    }

                    // Move the attacker back to it's starting position
                    timePassed = 0f;
                    while (timePassed < speed) {
                        attacker.transform.position = Vector3.Lerp(endPosition, startPosition, (timePassed / speed));
                        timePassed += Time.deltaTime;
                        yield return null;
                    }
                    attacker.transform.position = startPosition;


                    // Restore the attacker's position to a tile
                    attacker.transform.SetParent(actions[i].unit.transform);

                    yield return new WaitForSeconds(0.25f);
                }
            }

            // When all actions are resolved, clean the map
            map.GetComponent<Map>().Clean();
        }

        // Resolve combat
        yield return new WaitForSeconds(0.5f);

        End();
    }


    private IEnumerator ActivateMap(Callback callback) {
        float speed = 0.05f;

        List<int> indexes = map.GetComponent<Map>().GetTilesAvailable();

        // If no new indexes are to enabled, add a little delay
        if (indexes.Count == 0) {
            yield return new WaitForSeconds(speed);
        }

        for (int i=0; i<indexes.Count; i++) {
            Tile child_tile = map.transform.GetChild(indexes[i]).gameObject.GetComponent<Tile>();

            child_tile.isEnable = true;
            while (!child_tile.active.GetComponent<AnimationHandler>().isDone) {
                yield return null;
            }

            yield return new WaitForSeconds(speed);
        }

        callback();
    }

    public void Fight() {
        StartCoroutine(ResolveMap());
    }

    /*
    * Events
    */
    public void OnTurnEnded(GameObject unit) {
        cards.GetComponent<Cards>().Hide();

        StartCoroutine(ActivateMap(Fight));
    }
}

namespace UnityEngine.EventSystems {
    public interface ITurnEnded : IEventSystemHandler {
        void OnTurnEnded(GameObject unit);
    }
}
