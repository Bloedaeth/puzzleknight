﻿using System.Collections;
using UnityEngine;

public class Pylon : MonoBehaviour, IFreezable
{
    private static int numPylonsActive = 0;

    private float raiseLowerTime;
    public float RAISE_LOWER_TIME
    {
        get { return raiseLowerTime; }
        private set { raiseLowerTime = value; }
    }

    public int ID;
    public bool SlowedTime { get; set; }

    private Lever lever;
    private BossEnemy boss;
    private ParticleSystem ps;

    private bool isActive = false;

    private Vector3 maxHeightPos;
    private Vector3 minHeightPos;

    private const float MIN_HEIGHT = 456f;
    private const float MAX_HEIGHT = 480.5f;
    private const float BOSS_SCALE_INCREASE = 0.8f;

	private float distToMin;
	public float pylonScaleModifier;

    private void Start()
    {
        maxHeightPos = new Vector3(transform.localPosition.x, MAX_HEIGHT, transform.localPosition.z);
        minHeightPos = new Vector3(transform.localPosition.x, MIN_HEIGHT, transform.localPosition.z);

		distToMin = 0;

        lever = transform.parent.GetComponentInChildren<Lever>();
        boss = FindObjectOfType<BossEnemy>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(SlowedTime)
            RAISE_LOWER_TIME = 25f;
        else
            RAISE_LOWER_TIME = 5f;

		RecalculateScale ();
    }

	void RecalculateScale() { // Here's another method I'm using.
		distToMin = (-transform.localPosition + minHeightPos).magnitude;
		pylonScaleModifier = BOSS_SCALE_INCREASE * (distToMin / (MAX_HEIGHT - MIN_HEIGHT));
	}

    private IEnumerator RaiseLower(Vector3 endPos)
    {
        Vector3 start = transform.localPosition;
        float step = 0;
        while(step < 1f)
        {
            step += 1 / RAISE_LOWER_TIME * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, endPos, step);
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition = endPos;

        if(isActive)
            lever.SetLeverActive(true);
        else
            Invoke("AutoActivate", 5f);

        yield return null;
    }

    private void AutoActivate()
    {
        SetPylonActive(true);
    }

    public void ResetPylon()
    {
        CancelInvoke();
        StopAllCoroutines();
        if(isActive)
        {
            ps.Stop();
            isActive = false;
            --numPylonsActive;
        }
        transform.localPosition = minHeightPos;
    }

    public void SetPylonActive(bool val)
    {
        isActive = val;

        if(isActive)
        {
            ++numPylonsActive;
            StartCoroutine(RaiseLower(maxHeightPos));
			//boss.ScaleOverTime(BOSS_SCALE_INCREASE, this);  // Don't think we need this - Steve (GO TO LINE 96 in BossEnemy class)
            ps.Play();
        }
        else
        {
            --numPylonsActive;
            StartCoroutine(RaiseLower(minHeightPos));
            //boss.ScaleOverTime(-BOSS_SCALE_INCREASE, this);//
            ps.Stop();
        }
    }
}
