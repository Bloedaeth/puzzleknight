using System.Collections;
using System.Linq;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    /// <summary>The scaled flow of time in the freeze radius.</summary>
    public static float FROZEN_TIME_SCALE { get { return 0.1f; } }

    public bool freezeUsed;

    private ParticleSystem ps;
    private ParticleSystem.ShapeModule particleShape;

    private const float EXPANSION_RATE_MULT = 3f;
    private const float EMISSION_RATE = 50000f;
    private float radius;
    private bool active;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.maxParticles = main.maxParticles;

        ParticleSystem.EmissionModule emit = ps.emission;
        emit.rateOverTimeMultiplier = EMISSION_RATE;

        particleShape = ps.shape;
    }

    /// <summary>Freezes/Slows time in a set radius around the casting point for a set time.</summary>
    /// <param name="time">How long the effect will last once fully expanded.</param>
    /// <param name="radius">How far the effect will spread.</param>
    public void FreezeTime(float time, float radius)
    {
        StartCoroutine(ExpandFreezeRadius(time, radius));
    }

    private void Update()
    {
        if(!active)
            return;

        Transform[] freezable = FindObjectsOfType<Transform>().Where(o => o.GetComponent<IFreezable>() != null).ToArray();
        foreach(Transform freeze in freezable)
        {
            //Debug.Log(radius + " : " + Vector3.Distance(transform.position, freeze.position) + " : " + freeze.name);
            FreezeObj(freeze, Vector3.Distance(transform.position, freeze.position) < radius);
        }
    }

    private void FreezeObj(Transform other, bool frozenState)
    {
        IFreezable obj = other.GetComponent<IFreezable>();
        if(obj == null)
            return;

        obj.SlowedTime = frozenState;
    }

    private IEnumerator ExpandFreezeRadius(float time, float radius)
    {
        freezeUsed = true;
        active = true;
        ps.Play();

        float step = 0;
        float rate = 1 / time * EXPANSION_RATE_MULT;
        float start = 0.1f;
        float end = radius;
        while(step < 1f)
        {
            step += rate * Time.deltaTime;
            float curRad = Mathf.Lerp(start, end, step);
            this.radius = curRad;
            particleShape.radius = curRad;
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(time);

        step = 0;
        while(step < 1f)
        {
            step += rate * Time.deltaTime;
            float curRad = Mathf.Lerp(end, start, step);
            this.radius = curRad;
            particleShape.radius = curRad;
            yield return new WaitForFixedUpdate();
        }

        freezeUsed = false;
        active = false;
        ps.Stop();

        yield return null;
    }
}
