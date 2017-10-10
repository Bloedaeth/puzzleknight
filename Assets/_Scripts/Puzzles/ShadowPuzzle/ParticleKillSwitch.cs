using GameLogging;
using UnityEngine;

public class ParticleKillSwitch : MonoBehaviour {
	public void KillAllShadowParticles() {
        BuildDebug.Log("Killing all shadow particles.");
		Destroy (gameObject);
	}
}
