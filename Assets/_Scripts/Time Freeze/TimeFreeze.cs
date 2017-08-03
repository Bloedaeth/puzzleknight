using System.Collections;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    /// <summary>The scaled flow of time in the freeze radius.</summary>
    public static float FROZEN_TIME_SCALE = 0.1f;

    private new SphereCollider collider;
    private TimeFreezeCountdown timer;

    private void Awake()
    {
        collider = FindObjectOfType<TimeFreezeCollider>().GetComponent<SphereCollider>();
        collider.gameObject.SetActive(false);
        collider.enabled = true;
        timer = FindObjectOfType<TimeFreezeCountdown>();
    }

    /// <summary>Freezes/Slows time in a set radius around the casting point for a set time.</summary>
    /// <param name="time">How long the effect will last.</param>
    /// <param name="radius">How far the effect will spread.</param>
    public void FreezeTime(float time, float radius)
    {
        StartCoroutine(ActivateFreeze(time, radius));
    }

    private IEnumerator ActivateFreeze(float time, float radius)
    {
        collider.transform.position = transform.position + transform.forward + new Vector3(0, 1, 0);
        collider.radius = radius;
        collider.gameObject.SetActive(true);

        timer.TimerStart = time;
        timer.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        collider.GetComponent<TimeFreezeCollider>().EndFreeze();
    }
}
