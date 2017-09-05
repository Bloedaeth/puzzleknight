using UnityEngine;

public static class PlayPrefs
{
    private const string GAME_VOLUME_KEY = "Volume";

    public const float DEFAULT_VOLUME = 0.5f;

    public static float Volume
    {
        get { return PlayerPrefs.HasKey(GAME_VOLUME_KEY) ? PlayerPrefs.GetFloat(GAME_VOLUME_KEY) : DEFAULT_VOLUME; }
        set { PlayerPrefs.SetFloat(GAME_VOLUME_KEY, Mathf.Clamp(value, 0f, 1f)); }
    }
}
