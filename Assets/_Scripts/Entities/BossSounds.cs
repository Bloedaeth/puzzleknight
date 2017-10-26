using UnityEngine;

public class BossSounds : EntitySoundsCommon
{
	public AudioClip[] bigSwordSwingSounds;

    /// <summary>A list of sounds to be played when the boss is damaged at small scale.</summary>
    public AudioClip[] hurtSize1;

    /// <summary>A list of sounds to be played when the boss is damaged at medium scale.</summary>
    public AudioClip[] hurtSize2;

    /// <summary>A list of sounds to be played when the boss is damaged at large scale.</summary>
    public AudioClip[] hurtSize3;
}
