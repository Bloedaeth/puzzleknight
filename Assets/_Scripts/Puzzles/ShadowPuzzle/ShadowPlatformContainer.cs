using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPlatformContainer : MonoBehaviour {

	private ShadowModifier[] platforms;

	public GameObject master;
	public GameObject evilMaster;

	//private ShadowGradientStore sgs;

	// Use this for initialization
	void Start () {
		platforms = GetComponentsInChildren<ShadowModifier> (false);

		//sgs = GetComponent<ShadowGradientStore> ();

		ResetAllSystems ();
	}

	private void ResetAllSystems() {
		if (particlesExist ()) {
			BroadcastMessage ("KillAllShadowParticles");
		}

		for (int i = 0; i < platforms.Length; i++) {

			if (platforms [i].tag == "FalsePlatform") {
				Instantiate(evilMaster, platforms[i].transform);
			} else {
				Instantiate(master, platforms[i].transform);
			}

			platforms [i].ResetParticleSystem ();
		}
	}

	private bool particlesExist() {
		for (int i = 0; i < platforms.Length; i++) {
			if (platforms [i].hasParticleSystem ()) {
				return true;
			}
		}

		return false;
	}
}
