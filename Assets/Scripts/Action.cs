using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private GameObject _unit;
    private GameObject _target;
    private string _type = "attack";

    public GameObject unit {
        get { return _unit; }
    }

    public GameObject target {
        get { return _target; }
    }

    public string type {
        get { return _type; }
    }

    public Action(string new_type, GameObject new_unit, GameObject new_target) {
        _type = new_type;
        _unit = new_unit;
        _target = new_target;
    }

}
