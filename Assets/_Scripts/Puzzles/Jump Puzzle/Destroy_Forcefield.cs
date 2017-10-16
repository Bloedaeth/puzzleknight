using GameLogging;
using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
    private new AudioSource audio;
	private ParticleSystem[] particles;
    private GameObject forceField;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
		particles = GetComponentsInChildren<ParticleSystem> ();
        forceField = GameObject.FindGameObjectWithTag("ForceField");
    }

    private void OnTriggerEnter(Collider o)
    {
<<<<<<< HEAD
        if(o.CompareTag("Player") && forceField)
        {
            BuildDebug.Log("Destroying force field");
            Destroy(forceField);
            forceField = null; //just in case
        	foreach(ParticleSystem p in particles)
				p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
=======
		if (o.CompareTag ("Player")) {
			Destroy (GameObject.FindGameObjectWithTag ("ForceField"));
			foreach (ParticleSystem p in particles) {
				ParticleSystem.EmissionModule em = p.emission;
				em.enabled = false;
			}
		}

>>>>>>> Particle_Efficiency_Enhancements
        audio.Play();
    }

    private void OnTriggerExit(Collider o)
    {
        audio.Play();
    }
}


