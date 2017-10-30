using GameLogging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PressurePlateLower : MonoBehaviour
{
    private List<GameObject> entitiesOnPlate = new List<GameObject>();
    private bool lowered = false;

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
        audio.clip = stepOff;
    }

    private void Update()
    {
        entitiesOnPlate = entitiesOnPlate.Where(obj => obj.activeInHierarchy).ToList();

        if(entitiesOnPlate.Count > 0 && audio.clip != stepOn)
        {
            BuildDebug.Log("Playing pressure plate on sound");
            transform.position = lowerPos;

            audio.clip = stepOn;
            audio.Play();
        }
        else if(entitiesOnPlate.Count == 0 && audio.clip != stepOff)
        {
            BuildDebug.Log("Playing pressure plate off sound");
            transform.position = upperPos;

            audio.clip = stepOff;
            audio.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
		if((other.GetComponent<Entity>() == null && other.GetComponent<MovableObject>() == null) || other.GetComponent<MeleeWeapon>())
			return;

        entitiesOnPlate.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if((other.GetComponent<Entity>() == null && other.GetComponent<MovableObject>() == null) || other.GetComponent<MeleeWeapon>())
            return;

        entitiesOnPlate.Remove(other.gameObject);
    }
}