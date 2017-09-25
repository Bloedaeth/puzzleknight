using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour {
    public GameObject other;
    void OnCollisionEnter(Collision collidingObject)
    {
        
        //If collidingObject is an Enemy
        if (collidingObject.gameObject.tag == "Player")
        {
            Destroy(other);
        }

        ;
    }
}


