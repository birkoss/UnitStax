using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager> {
    protected PrefabManager() {}

    [System.Serializable]
    public class PrefabManagerEntry {
        public string entry;
        public GameObject prefab;
    }

    public PrefabManagerEntry[] entries;

    public GameObject Get(string entry) {
        for (int i=0; i<entries.Length; i++) {
            if (entries[i].entry == entry) {
                return entries[i].prefab;
            }
        }
        return null;
    }
}
