using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DetailDistance : MonoBehaviour {

    public float distance = 250; // 250 is max in terrain settings, but not here
    Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("This gameobject is not terrain, disabling forced details distance", gameObject);
            this.enabled = false;
            return;
        }

    }

    // WARNING: this runs update loop inside editor, you dont need this if you dont change the value
    void Update()
    {
        terrain.detailObjectDistance = distance;
    }
}
