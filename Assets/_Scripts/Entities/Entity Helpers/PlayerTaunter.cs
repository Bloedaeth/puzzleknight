using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerTaunter : MonoBehaviour {
	public AudioSource a { get { return GetComponent<AudioSource> (); } }
}
