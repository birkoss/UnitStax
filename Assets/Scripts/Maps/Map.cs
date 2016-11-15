using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IUnitPlaced {

    private int rows = 6;
    private int cols = 6;

    private List<int> units;


    public GameObject[] available {
        get {
            List<GameObject> tiles = new List<GameObject>();
            for (int i=0; i<transform.childCount; i++) {
                if (transform.GetChild(i).gameObject.GetComponent<Tile>().isEnable && transform.GetChild(i).gameObject.GetComponent<Tile>().isEmpty) {
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
        units = new List<int>();

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
    public List<int> GetTilesAvailable() {
        List<int> tiles = new List<int>();

        bool isEmpty = true;

        List<int> neighboors;

        List<int> indexes = new List<int>();
        for (int i=0; i<transform.childCount; i++) {
            if (!transform.GetChild(i).gameObject.GetComponent<Tile>().isEmpty) {
                isEmpty = false;
                neighboors = GetNeighboors(transform.GetChild(i).gameObject.GetComponent<Tile>().position);
                for (int j=0; j<neighboors.Count; j++) {
                    GameObject tile = transform.GetChild(neighboors[j]).gameObject;
                    if (tile.GetComponent<Tile>().isEmpty && !tile.GetComponent<Tile>().isEnable) {
                        tiles.Add(neighboors[j]);
                    }
                }
            }
        }

        if (isEmpty && tiles.Count == 0) {
            tiles.Add(Random.Range(0, transform.childCount-1));

            // Get all neighboors
            neighboors = GetNeighboors(transform.GetChild(tiles[0]).gameObject.GetComponent<Tile>().position);
            for (int i=0; i<neighboors.Count; i++) {
                if (transform.GetChild(neighboors[i]).gameObject.GetComponent<Tile>().isEmpty) {
                    tiles.Add(neighboors[i]);
                }
            }
        }

        return tiles;
    }


    public List<Action> GetActions() {
        List<Action> actions = new List<Action>();

        for (int i=units.Count - 1; i>=0; i--) {
            int index = units[i];
            GameObject tile = transform.GetChild(index).gameObject;
            if (!tile.GetComponent<Tile>().isEmpty) {
                if (tile.transform.GetChild(2).GetComponent<Unit>().isAlive) {

                    var tilePlayer = tile.transform.GetChild(2).GetComponent<Unit>().player;

                    List<GameObject> enemies = new List<GameObject>();
                    List<int> indexes = GetNeighboors(tile.GetComponent<Tile>().position);
                    GameObject otherTile;
                    for (int j=0; j<indexes.Count; j++) {
                        otherTile = transform.GetChild(indexes[j]).gameObject;
                        if (!otherTile.GetComponent<Tile>().isEmpty && otherTile.transform.GetChild(2).GetComponent<Unit>().player != tilePlayer) {
                            enemies.Add(otherTile);
                        }
                    }

                    if (enemies.Count > 0) {
                        Debug.Log("Put back 2 here...");
                        for (int j=0; j<enemies.Count; j++) {
                            // actions.Add(new Action("attack", enemies[j], tile));
                            actions.Add(new Action("attack", tile, enemies[j]));
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
                if (!tile.transform.GetChild(2).GetComponent<Unit>().isAlive) {
                    Destroy(tile.transform.GetChild(2).gameObject);
                    units.Remove(i);
                }
            }
        }
    }


    /*
    * Events
    */
    public void OnUnitPlaced(GameObject unit) {
        //unit.GetComponent<Animator>().enabled = true;
        //unit.GetComponent<DragHandler>().enabled = false;
        Debug.Log(unit.transform.parent);

        GameObject tile = unit.transform.parent.gameObject;
        int index = (int)((tile.GetComponent<Tile>().position.y * rows) + tile.GetComponent<Tile>().position.x);
        units.Add(index);
    }
}


namespace UnityEngine.EventSystems {
    public interface IUnitPlaced : IEventSystemHandler {
        void OnUnitPlaced(GameObject unit);
    }
}
