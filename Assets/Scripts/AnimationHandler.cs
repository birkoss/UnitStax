using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

    private bool _isDone = false;

    public bool isDone {
        get { return _isDone; }
        set { _isDone = value; }
    }

    public void Ended() {
        isDone = true;
    }
}
