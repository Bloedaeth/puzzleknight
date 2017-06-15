using UnityEngine;

public class EntitySoundsCommon : MonoBehaviour
{
    /// <summary>A list of sounds to be played when the entity's health is depleted.</summary>
    public AudioClip[] deathSounds;

    /// <summary>A list of sounds to be played when damage is taken.</summary>
    public AudioClip[] hurtSounds;

    /// <summary>A list of sounds to be played when the entity executes an attack.</summary>
    public AudioClip[] attackSounds;

    /// <summary>A list of taunts to be played during or after combat.</summary>
    public AudioClip[] taunts;

    /// <summary>A list of sounds to be played when idle or roaming.</summary>
    public AudioClip[] idleSounds;
}
