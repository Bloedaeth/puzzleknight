using GameLogging;
using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
	public Transform particleContainer;
	private ParticleSystem[] particles;
    private GameObject forceField;

    private void Awake()
    {
		particles = particleContainer ? particleContainer.GetComponentsInChildren<ParticleSystem> () : GetComponentsInChildren<ParticleSystem> ();
        forceField = GameObject.FindGameObjectWithTag("ForceField");
    }

    private void OnTriggerEnter(Collider o)
    {
        if(o.CompareTag("Player") && forceField)
        {
            BuildDebug.Log("Destroying force field");
            Destroy(forceField);
            forceField = null; //just in case
        	foreach(ParticleSystem p in particles)
				p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
		if (o.CompareTag ("Player")) {
			Destroy (GameObject.FindGameObjectWithTag ("ForceField"));
			foreach (ParticleSystem p in particles) {
				ParticleSystem.EmissionModule em = p.emission;
				em.enabled = false;
			}
		}
    }
		
}


