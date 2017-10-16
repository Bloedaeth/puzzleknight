using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
    private new AudioSource audio;
	private ParticleSystem[] particles;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
		particles = GetComponentsInChildren<ParticleSystem> ();
    }

    private void OnTriggerEnter(Collider o)
    {
		if (o.CompareTag ("Player")) {
			Destroy (GameObject.FindGameObjectWithTag ("ForceField"));
			foreach (ParticleSystem p in particles) {
				ParticleSystem.EmissionModule em = p.emission;
				em.enabled = false;
			}
		}

        audio.Play();
    }

    private void OnTriggerExit(Collider o)
    {
        audio.Play();
    }
}


