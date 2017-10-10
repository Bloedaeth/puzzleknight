using GameLogging;
using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
    private new AudioSource audio;
    private GameObject forceField;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        forceField = GameObject.FindGameObjectWithTag("ForceField");
    }

    private void OnTriggerEnter(Collider o)
    {
        if(o.CompareTag("Player") && forceField)
        {
            BuildDebug.Log("Destroying force field");
            Destroy(forceField);
            forceField = null; //just in case
        }
        audio.Play();
    }

    private void OnTriggerExit(Collider o)
    {
        audio.Play();
    }
}


