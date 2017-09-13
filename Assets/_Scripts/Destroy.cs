using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public GameObject other;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collidingObject)
    {
        if (collidingObject.gameObject.tag =="Player")
        {
            Destroy(other);
        }

    }

}
