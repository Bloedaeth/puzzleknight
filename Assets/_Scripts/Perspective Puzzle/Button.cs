using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public bool playerStanding;

	private bool isActive;
	private bool deactivateCalled;
	private float deactivateTime;
	private float deactivateDelay = 2f;

	public GameObject cameraPoint;

	UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl tpuc;

	// Use this for initialization
	void Start () {
		isActive = true;
		deactivateCalled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (deactivateCalled) {
				if (Time.time > deactivateTime) {
					isActive = false;
				}
			}
		}
	}

	public void Deactivate() {
		deactivateTime = Time.time + deactivateDelay;
		deactivateCalled = true;
	}

	public GameObject GetCameraPoint() {
		return cameraPoint;
	}

	void OnTriggerEnter(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player" && isActive) {
			playerStanding = true;
			tpuc = o.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ();
		}
	}

	void OnTriggerExit(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player") {
			playerStanding = false;

			tpuc.isLooking = false;
			tpuc.ResetCamera ();
			tpuc.freeLookCamera.UpdateTarget (o.gameObject.transform);
		}
	}

	void OnTriggerStay(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player" && isActive) {
			tpuc.isLooking = true;
			tpuc.SetCameraToZeros();
			tpuc.freeLookCamera.UpdateTarget (cameraPoint.transform);
			tpuc.freeLookCamera.ToggleCamClip(true);
		}
	}
		
}
