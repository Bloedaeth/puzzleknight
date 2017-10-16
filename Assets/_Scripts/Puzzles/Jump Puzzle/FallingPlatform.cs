﻿using GameLogging;
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
        GetComponent<Collider>().isTrigger = true;
		yield return new WaitForSeconds(fallDelay);
		if (PS.particle) {
			ParticleSystem.EmissionModule em = PS.particle.emission;
			em.enabled = false;
		}
		PS.enabled = false;
        yield return 0;
    }
}
