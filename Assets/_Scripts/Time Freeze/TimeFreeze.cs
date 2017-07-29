using System.Collections;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    /// <summary>The scaled flow of time in the freeze radius.</summary>
    public static float FROZEN_TIME_SCALE = 0.1f;

    /// <summary>The game object that determines where time should freeze.</summary>
    public SphereCollider FreezeRadiusCollider;

    /// <summary>Freezes/Slows time in a set radius around the casting point for a set time.</summary>
    /// <param name="time">How long the effect will last.</param>
    /// <param name="radius">How far the effect will spread.</param>
    public void FreezeTime(float time, float radius)
    {
        StartCoroutine(ActivateFreeze(time, radius));
    }

    private IEnumerator ActivateFreeze(float time, float radius)
    {
        FreezeRadiusCollider.transform.position = transform.position + transform.forward + new Vector3(0, 1, 0);
        FreezeRadiusCollider.radius = radius;
        FreezeRadiusCollider.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        FreezeRadiusCollider.GetComponent<TimeFreezeCollider>().EndFreeze();
    }
}
