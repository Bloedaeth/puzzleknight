using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TerrainType : MonoBehaviour {

	public enum GroundType {none, grass, stone, wood}
	public GroundType thisType;

	public EntityFootstepSounds efs;

	public AudioClip footstepSound { get { return efs.RandomFootstepSound (thisType); } }

	// Use this for initialization
	void Start () {
		if (!efs) {
			efs = GetComponentInParent<EntityFootstepSounds> ();
		}

		if (thisType == null) {
			thisType = GroundType.none;
		}
	}
}
