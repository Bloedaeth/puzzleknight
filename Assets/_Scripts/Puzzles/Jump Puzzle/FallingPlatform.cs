using GameLogging;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlatformShake))]
public class FallingPlatform : MonoBehaviour {
    private Rigidbody rbody;
    public float fallDelay;
	private float respawnDelay = 1f;
	PlatformShake PS { get { return GetComponent<PlatformShake> (); } }

	Collider c;

    private void Start()
    {
        rbody = GetComponentInChildren<Rigidbody>();

		foreach (Collider co in GetComponents<Collider>()) {
			if (!co.isTrigger) {
				c = co;
				break;
			}
		}
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        BuildDebug.Log("Platform fall activated for " + name);
        PS.FallShake(fallDelay);
        yield return new WaitForSeconds(fallDelay);
        rbody.isKinematic = false;

        c.isTrigger = true;

        yield return new WaitForSeconds(fallDelay);
		if (PS.particle) {
			ParticleSystem.EmissionModule em = PS.particle.emission;
			em.enabled = false;
		}
		PS.enabled = false;

		yield return new WaitForSeconds(fallDelay+1+respawnDelay);
		ReturnPlatform ();

        yield return 0;
    }

	void ReturnPlatform() {

		PS.enabled = true;
		PS.ResetAfterFall ();
		rbody.isKinematic = true;
		rbody.transform.localRotation = Quaternion.Euler(Vector3.zero);
		rbody.transform.localPosition = Vector3.zero;
		c.isTrigger = false;

		if (PS.particle) {
			ParticleSystem.EmissionModule em = PS.particle.emission;
			em.enabled = true;
		}
	}
}
