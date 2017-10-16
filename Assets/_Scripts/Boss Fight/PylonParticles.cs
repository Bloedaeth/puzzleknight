using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonParticles : MonoBehaviour {

	ParticleSystem ps { get { return GetComponent<ParticleSystem> (); } }
	ParticleSystem.Particle[] particles;
	int aliveParticles;

	ParticleSystem.EmissionModule em;
	float origEmRate;

	[SerializeField] float acceleration = 0.8f;
	[Range(0f, 10f)][SerializeField] float maxSpeedFraction = 0.5f;
	[Range(0f, 10f)][SerializeField] float distanceToStartDying = 1f;

	public Transform hook;

	// Use this for initialization
	void Start () {

		if (!hook)
			hook = transform;

		particles = new ParticleSystem.Particle[ps.main.maxParticles];

		em = ps.emission;
		origEmRate = em.rateOverTime.constant;

		print (origEmRate.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		aliveParticles = ps.GetParticles (particles);

		for (int i = 0; i < aliveParticles; i++) {
			Vector3 vec = (-particles[i].position + hook.position);

			if (vec.magnitude > distanceToStartDying && particles [i].remainingLifetime <= particles[i].startLifetime * 0.8f) {
				particles [i].remainingLifetime += Time.deltaTime;
			}

			particles[i].velocity += vec.normalized * acceleration;

			if (particles [i].velocity.magnitude > vec.magnitude * maxSpeedFraction) {
				particles [i].velocity = particles [i].velocity.normalized * vec.magnitude * maxSpeedFraction;
			}

			ps.SetParticles (particles, aliveParticles);
		}
	}

	public void SetEmissionMult(float mult) {
		

		if (mult > 1f) {
			mult = 1f;
		} else if (mult < 0f) {
			mult = 0f;
			ps.Stop ();
		} else if (!ps.isPlaying){
			ps.Play ();
		}

		em.rateOverTime = origEmRate * mult;

		print (mult.ToString());
	}
}
