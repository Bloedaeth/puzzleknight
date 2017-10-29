using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFootstepSounds : MonoBehaviour {

	// When walking on grass
	public AudioClip[] grassFootsteps;

	// When walking on stone
	public AudioClip[] stoneFootsteps;

	// When walking on wooden surfaces
	public AudioClip[] woodFootsteps;

	// When walking on sunshine, whoooooah-oh
	public AudioClip[] genericFootsteps;

	public AudioClip RandomFootstepSound(TerrainType.GroundType gt) {
		switch (gt) {
		case TerrainType.GroundType.grass:
			if (grassFootsteps.Length > 0) {
				return grassFootsteps [Random.Range (0, grassFootsteps.Length)];
			} 
			break;

		case TerrainType.GroundType.stone:
			if (stoneFootsteps.Length > 0) {
				return stoneFootsteps [Random.Range (0, stoneFootsteps.Length)];
			} 
			break;
		
		case TerrainType.GroundType.wood:
			if (woodFootsteps.Length > 0) {
				return woodFootsteps [Random.Range (0, woodFootsteps.Length)];
			} 
			break;
		}

		return ReturnGenericSound ();
	}

	public AudioClip ReturnGenericSound() {
		if (genericFootsteps.Length > 0) {
			return genericFootsteps [Random.Range (0, genericFootsteps.Length)];
		} 
		return null;
	}
}
