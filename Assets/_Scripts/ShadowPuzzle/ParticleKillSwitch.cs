using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKillSwitch : MonoBehaviour {
	public void KillAllShadowParticles() {
		Destroy (gameObject);
	}
}
