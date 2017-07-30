using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform myTransform;

	//Enemy Player reference
	private GameObject enemyPlayer;

	//Turret Variables
	public float range = 100.0f;
	public float fireRate = 20.0f;
	private float fireTime;

	//Projectile
	public GameObject turretProjectile;
	
	//Turret Parts
	public GameObject turretMuzzle;
	public GameObject turretRaycast;

	//Rotation Variables
	public float rotationSpeed = 2.0f;
	private float adjRotSpeed;
	private Quaternion targetRotation;

	// Use this for initialization
	void Start () {
		myTransform = this.transform;

		enemyPlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		//Raycast Detection
		RaycastHit hit;
		if (Physics.Raycast (turretRaycast.transform.position, -(turretRaycast.transform.position - enemyPlayer.transform.position).normalized, out hit, range)) {

			//If hit has "Player" tag...
			if (hit.transform.tag == "Player"){
                				
				//Fire Projectile
				if (Time.time > fireTime) {
					Instantiate (turretProjectile, turretMuzzle.transform.position, turretMuzzle.transform.rotation);
					fireTime = Time.time + fireRate;
				}

				//Draw red debug line
				Debug.DrawLine (turretRaycast.transform.position, hit.point, Color.red);
			} else {
				//Draw green debug line
				Debug.DrawLine (turretRaycast.transform.position, hit.point, Color.green);
			}
		}


	}
}
