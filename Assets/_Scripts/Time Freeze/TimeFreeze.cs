using System.Collections;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    /// <summary>The scaled flow of time in the freeze radius.</summary>
    public static float FROZEN_TIME_SCALE { get { return 0.1f; } }

    private const float EXPANSION_RATE = 0.3f;
    private const float EMISSION_RATE = 50000f;
    //private const float GAME_SCALE = 6f;

    /// <summary>The game object that determines where time should freeze.</summary>
    public new SphereCollider collider;
    private ParticleSystem.ShapeModule particleShape;

    public bool freezeUsed;

    private void Awake()
    {
        ParticleSystem ps = collider.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.maxParticles = main.maxParticles /* * (int)GAME_SCALE*/;

        ParticleSystem.EmissionModule emit = ps.emission;
        emit.rateOverTimeMultiplier = EMISSION_RATE /* * GAME_SCALE*/;

        particleShape = ps.shape;
    }

    /// <summary>Freezes/Slows time in a set radius around the casting point for a set time.</summary>
    /// <param name="time">How long the effect will last once fully expanded.</param>
    /// <param name="radius">How far the effect will spread.</param>
    public void FreezeTime(float time, float radius)
    {
        collider.transform.position = transform.position;// + transform.forward + new Vector3(0, 1, 0);
        collider.gameObject.SetActive(true);

        StartCoroutine(ExpandFreezeRadius(time, radius));
    }

    private IEnumerator ExpandFreezeRadius(float time, float radius)
    {
        freezeUsed = true;
        for (float i = collider.radius; i <= radius /* * GAME_SCALE*/; i += EXPANSION_RATE /* * GAME_SCALE*/)
        {
            collider.radius = i;
            particleShape.radius = i;
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(time);
        
        for(float i = collider.radius; i >= 0.1f; i -= EXPANSION_RATE /* * GAME_SCALE*/)
        {
            collider.radius = i;
            particleShape.radius = i;
            yield return new WaitForFixedUpdate();
        }
        freezeUsed = false;
        collider.gameObject.SetActive(false);
    }
}
