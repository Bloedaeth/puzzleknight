using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour {

		
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")        {
            other.transform.parent = transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            other.transform.parent = null;
        }
    }
}
