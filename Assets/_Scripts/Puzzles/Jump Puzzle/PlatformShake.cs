using UnityEngine;

public class PlatformShake : MonoBehaviour
{
    public float shake_speed;
    public float shake_intensity;
    public Vector3 originPosition;

    float shakeTime;
    float shakeRateMin = 1f;
    float shakeRateMax = 2f;
    float shakePause { get { return Random.Range(shakeRateMin, shakeRateMax * 4); } }
    float shakeRate { get { return Random.Range (shakeRateMin, shakeRateMax); } }
	float currShakeRate;

	float shakeStage { get { return (Time.time - shakeTime) / currShakeRate; } }
	float shakeMaxAngle = 1f;

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
	}

	public void FallShake(float delay) {
		isAboutToFall = true;
		currShakeRate = delay * 2f;
		shakeTime = Time.time;
		shakeMaxAngle *= 4f;
		maxParticleRate *= 2f;
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

		transform.rotation = originalRotation;

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

		transform.rotation = Quaternion.Euler (originalRotation.eulerAngles + new Vector3 (x, 0f, z));


	}

	void EndShake() {
		isShaking = false;
		shakeTime = Time.time + shakePause;
	}

	/*
    void Start()
    {
        originPosition = transform.position;
    }

    private void Update()
    {
        if(isShaking)
        {
            float step = shake_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, originPosition + Random.insideUnitSphere, step);
        }
    }
    */
}
