﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShake : MonoBehaviour {

	Transform shaker;

	float shakeTime;
	float shakeRateMin = 1f;
	float shakeRateMax = 3f;
	float shakePause { get { return Random.Range (shakeRateMin, shakeRateMax*2); } }

	float shakeRate { get { return Random.Range (shakeRateMin, shakeRateMax); } }
	float currShakeRate;

	float shakeStage { get { return (Time.time - shakeTime) / currShakeRate; } }
	float shakeMaxAngle = 2f;

	ParticleSystem p;
	public ParticleSystem particle { get { return p; } }
	ParticleSystem.EmissionModule pem;
	float maxParticleRate = 50f;

	Quaternion originalRotation;

    bool isShaking;
	bool isAboutToFall;

	void Awake() {
		originalRotation = transform.rotation;
		p = GetComponentInChildren<ParticleSystem> ();
        if(p)
            pem = p.emission;
		
		shaker = GetComponentsInChildren<Transform> () [1];

	}

	void OnDisable() {
		shaker.rotation = originalRotation;
	}

	public void FallShake(float delay) {
		isAboutToFall = true;
		currShakeRate = delay * 2f;
		shakeTime = Time.time;
		shakeMaxAngle *= 2f;
		maxParticleRate *= 2f;
	}

	public void ResetAfterFall() {
		isAboutToFall = false;
		shakeMaxAngle /= 2f;
		maxParticleRate /= 2f;
		currShakeRate = shakeRate;
	}

	void Update() {
		if (Time.time > shakeTime && !isShaking && !isAboutToFall) {
			currShakeRate = shakeRate;
			isShaking = true;
		}

		if (isShaking || isAboutToFall) {
			ShakePlatform (shakeStage);
		}
	}

	void ShakePlatform(float stage) {

		shaker.rotation = originalRotation;

        if(p)
            pem.rateOverTime = 0;

		if (stage > 1 || stage < 0) {
			EndShake ();
			return;
		}

		float cosStage = (-0.5f * Mathf.Cos (2f * Mathf.PI * stage) + 0.5f);

        if(p)
            pem.rateOverTime = cosStage * maxParticleRate;

		float x = 0;
		float z = 0;

		float max = shakeMaxAngle * cosStage;

		x = Random.Range (-max, max);
		z = Random.Range (-max, max);

		shaker.rotation = Quaternion.Euler (originalRotation.eulerAngles + new Vector3 (x, 0f, z));


	}

	void EndShake() {
		isShaking = false;
		shakeTime = Time.time + shakePause;
	}
}
