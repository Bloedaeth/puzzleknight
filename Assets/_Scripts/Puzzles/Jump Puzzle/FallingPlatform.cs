using GameLogging;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlatformShake))]
public class FallingPlatform : MonoBehaviour {
    private Rigidbody rbody;
    public float fallDelay;
	PlatformShake PS { get { return GetComponent<PlatformShake> (); } }

    private void Start()
    {
        rbody = GetComponentInChildren<Rigidbody>();
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
        foreach(Collider c in GetComponentsInParent<Collider>())
            c.isTrigger = true;
        if(PS.particle)
            PS.particle.Stop ();
		PS.enabled = false;
        yield return 0;
    }
}
