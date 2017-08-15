using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

    public float time = 0.0f;
    public GameObject player;
    public GameObject scene;
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(time >= 10.0f)
        {
            player.gameObject.SetActive(true);
            scene.gameObject.SetActive(false);
        }
	}
}
