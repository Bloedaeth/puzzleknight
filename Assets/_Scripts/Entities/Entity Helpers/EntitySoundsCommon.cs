using UnityEngine;

public class EntitySoundsCommon : MonoBehaviour
{
    /// <summary>A list of sounds to be played when the entity's health is depleted.</summary>
    public AudioClip[] deathSounds;

    /// <summary>A list of sounds to be played when damage is taken.</summary>
    public AudioClip[] hurtSounds;

    /// <summary>A list of sounds to be played when the entity's attack hits.</summary>
    public AudioClip[] attackHitSounds;

	/// <summary>A list of sounds to be played when the entity swings its sword.</summary>
	public AudioClip[] swordSwingSounds;

    /// <summary>A list of taunts to be played during or after combat.</summary>
    //public AudioClip[] taunts;

    /// <summary>A list of sounds to be played when idle or roaming.</summary>
    //public AudioClip[] idleSounds;
    
    /// <summary>A list of sounds to be played when the entity walks.</summary>
    //public AudioClip[] walkSounds;
}
