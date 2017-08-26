using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcefield_Sound_Trigger : MonoBehaviour {
    public AudioClip otherClip;
    void OnTriggerEnter(Collider other)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
    }
}
