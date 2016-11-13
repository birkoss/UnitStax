using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IUnitPlaced {

    private int rows = 6;
    private int cols = 3;

    private Turn.CallBack callback;

    public GameObject[] active {
        get {
            List<GameObject> tiles = new List<GameObject>();
            for (int i=0; i<transform.childCount; i++) {
                if (transform.GetChild(i).gameObject.GetComponent<Tile>().isActive) {
                    tiles.Add(transform.GetChild(i).gameObject);
                }
            }
            return tiles.ToArray();
        }
    }


    /*
    * Unity
    */
	void Awake () {
        // Create remaining tiles
        for (int i=1; i<(rows * cols); i++) {
            Instantiate(transform.GetChild(0).gameObject, transform);
        }

        // Set the correct X, Y, and NAME
        for (int i=0; i<transform.childCount; i++) {
            int y = (i / rows);
            int x = i - (y * rows);
            transform.GetChild(i).gameObject.GetComponent<Tile>().position = new Vector2(x, y);
            transform.GetChild(i).gameObject.name = x + "x" + y;
        }
	}


    /*
    * Methods
    */
    public void EnableMovement(Turn.CallBack new_callback) {
        callback = new_callback;

        List<int> indexes = new List<int>();
        for (int i=0; i<transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.GetComponent<Tile>().isEmpty) {
                indexes.Add(i);
            }
        }

        int index = indexes[Random.Range(0, indexes.Count-1)];
        EnableTile(index);
    }


    public void DisableMovement() {
        for (int i=0; i<transform.childCount; i++) {
            transform.GetChild(i).gameObject.GetComponent<Tile>().isActive = false;
        }
    }


    public void EnableTile(int index, bool mainTile = true) {
        GameObject tile = transform.GetChild(index).gameObject;
        tile.GetComponent<Tile>().isActive = true;

        if (mainTile) {
            StartCoroutine(ChooseOtherTiles(GetNeighboors(tile.GetComponent<Tile>().position)));
        }
    }


    public List<Action> GetActions() {
        List<Action> actions = new List<Action>();

        for (int i=0; i<transform.childCount; i++) {
            GameObject tile = transform.GetChild(i).gameObject;
            if (!tile.GetComponent<Tile>().isEmpty) {
                if (tile.transform.GetChild(2).GetComponent<Alive>().isAlive) {

                    var tilePlayer = tile.transform.GetChild(2).GetComponent<Alive>().player;

                    List<GameObject> enemies = new List<GameObject>();
                    List<int> indexes = GetNeighboors(tile.GetComponent<Tile>().position);
                    GameObject otherTile;
                    for (int j=0; j<indexes.Count; j++) {
                        otherTile = transform.GetChild(indexes[j]).gameObject;
                        if (!otherTile.GetComponent<Tile>().isEmpty && otherTile.transform.GetChild(2).GetComponent<Alive>().player != tilePlayer) {
                            enemies.Add(otherTile);
                        }
                    }

                    if (enemies.Count > 0) {
                        Debug.Log("Put back 2 here...");
                        for (int j=0; j<enemies.Count; j++) {
                            actions.Add(new Action("attack", enemies[j], tile));
                        }
                    }
                }

            }
        }

        return actions;
    }


    public List<int> GetNeighboors(Vector2 position) {
        List<int> indexes = new List<int>();
        int index = (int)((position.y * rows) + position.x);

        if (position.x>0) {
            indexes.Add(index - 1);
        }
        if (position.x < (rows - 1)) {
            indexes.Add(index + 1);
        }
        if (position.y > 0) {
            indexes.Add(index - rows);
        }
        if (position.y < (cols - 1)) {
            indexes.Add(index + rows);
        }

        return indexes;
    }


    public void Clean() {
        for (int i=0; i<transform.childCount; i++) {
            GameObject tile = transform.GetChild(i).gameObject;
            if (!tile.GetComponent<Tile>().isEmpty) {
                if (!tile.transform.GetChild(2).GetComponent<Alive>().isAlive) {
                    Destroy(tile.transform.GetChild(2).gameObject);
                }
            }
        }
    }

    /*
    * Coroutine
    */
    IEnumerator ChooseOtherTiles(List<int> indexes) {
        float speed = 0.25f;

        yield return new WaitForSeconds(speed);
        for (int i=0; i<indexes.Count; i++) {
            Tile child_tile = transform.GetChild(indexes[i]).gameObject.GetComponent<Tile>();
            if (child_tile.isEmpty) {
                EnableTile(indexes[i], false);
            }
            yield return new WaitForSeconds(speed);
        }

        if (callback != null) {
            callback();
        }
    }


    private IEnumerator DeactivateTiles() {
        yield return new WaitForSeconds(0.25f);

        for (int i=0; i<transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.GetComponent<Tile>().isActive) {
                transform.GetChild(i).gameObject.GetComponent<Tile>().active.GetComponent<Animator>().Play("FadeOff");
                yield return new WaitForSeconds(0.25f);
                transform.GetChild(i).gameObject.GetComponent<Tile>().isActive = false;
            }
        }

        yield return new WaitForSeconds(0.25f);

        ExecuteEvents.ExecuteHierarchy<ITurnEnded>(gameObject, null, (x,y) => x.OnTurnEnded());
    }


    /*
    * Events
    */
    public void OnUnitPlaced(GameObject unit) {
        unit.GetComponent<Animator>().enabled = true;
        Debug.Log("OnUnitPlaced...");
        //DisableMovement();
        StartCoroutine(DeactivateTiles());
    }
}


namespace UnityEngine.EventSystems {
    public interface IUnitPlaced : IEventSystemHandler {
        void OnUnitPlaced(GameObject unit);
    }
}
