using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRoomScript : MonoBehaviour {

	public GameObject ShadowEnemy;
	public GameObject ShadowEntity;

	public PointStore pnts;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			GameObject.Instantiate (ShadowEntity, pnts.GetRandomPoint ().transform.position, pnts.GetRandomPoint ().transform.rotation);
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			GameObject.Instantiate (ShadowEnemy, pnts.GetRandomPoint ().transform.position, pnts.GetRandomPoint ().transform.rotation);
		}
			
	}
}
