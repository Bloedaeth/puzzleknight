using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    /// <summary>The scaled flow of time in the freeze radius.</summary>
    public static float FROZEN_TIME_SCALE = 0.1f;

    /// <summary>The game object that determines where time should freeze.</summary>
    public SphereCollider TimeFreezeCollider;

    private List<GameObject> frozenObjects = new List<GameObject>();

    /// <summary>Freezes/Slows time in a set radius around the casting point for a set time.</summary>
    /// <param name="time">How long the effect will last.</param>
    /// <param name="radius">How far the effect will spread.</param>
    public void FreezeTime(float time, float radius)
    {
        StartCoroutine(ActivateFreeze(time, radius));
    }

    private IEnumerator ActivateFreeze(float time, float radius)
    {
        TimeFreezeCollider.transform.position = transform.position + transform.forward + new Vector3(0, 1, 0);
        TimeFreezeCollider.radius = radius;
        TimeFreezeCollider.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);
        
        for(int i = 0; i < frozenObjects.Count; ++i)
        {
            frozenObjects[i].GetComponent<Enemy>().SlowedTime = false;
            frozenObjects.Remove(frozenObjects[i]);
        }
        TimeFreezeCollider.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy == null)
            return;

        if(enemy.SlowedTime)
            return;

        enemy.SlowedTime = true;
        frozenObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy == null)
            return;

        enemy.SlowedTime = false;
        frozenObjects.Remove(other.gameObject);
    }
}
