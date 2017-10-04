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
		
        ///longer delay (2 seconds?) and a "shake" animation before falling would be good at some point
		PS.FallShake (fallDelay); // Ask and ye shall recieve ~Steve
        yield return new WaitForSeconds(fallDelay);
        rbody.isKinematic = false;
        GetComponent<Collider>().isTrigger = true;
		PS.particle.Stop ();
		PS.enabled = false;
        yield return 0;
    }
}
