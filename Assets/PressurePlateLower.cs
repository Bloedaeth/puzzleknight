using GameLogging;
using UnityEngine;

public class PressurePlateLower : MonoBehaviour
{
    private int numEntitiesOnPlate = 0;
    private bool lower = false;

    private Vector3 upperPos;
    private Vector3 lowerPos;

    private new AudioSource audio;

    public AudioClip stepOn;
    public AudioClip stepOff;

    private void Awake()
    {
        upperPos = transform.position;
		lowerPos = upperPos - Vector3.up/20;
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(lower)
            transform.position = lowerPos;
        else
            transform.position = upperPos;
    }

    private void OnTriggerEnter(Collider other)
    {
		if ((other.GetComponent<Entity> () == null && other.GetComponent<MovableObject> () == null) || other.GetComponent<MeleeWeapon> ()) {
			return;
		}

        ++numEntitiesOnPlate;
        if(!lower)
        {
            BuildDebug.Log("Playing pressure plate off sound");
            lower = true;

            audio.clip = stepOn;
			audio.Play ();
        }
    }

    private void OnTriggerExit(Collider other)
    {
		if ((other.GetComponent<Entity> () == null && other.GetComponent<MovableObject> () == null) || other.GetComponent<MeleeWeapon> ()) {
			return;
		}

        --numEntitiesOnPlate;
        if(numEntitiesOnPlate == 0)
        {
            BuildDebug.Log("Playing pressure plate off sound");
            lower = false;

			audio.clip = stepOff;
			audio.Play ();
        }
    }
}