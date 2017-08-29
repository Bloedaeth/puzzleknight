using UnityEngine;

public static class PlayPrefs
{
    private const string GAME_VOLUME_KEY = "Volume";
    private const string DIFFICULTY_KEY = "Difficulty";

    public static float Volume
    {
        get { return PlayerPrefs.GetFloat(GAME_VOLUME_KEY); }
        set { if(value >= 0f && value <= 1f) PlayerPrefs.SetFloat(GAME_VOLUME_KEY, value); }
    }

    public static float Difficulty
    {
        get { return PlayerPrefs.GetFloat(DIFFICULTY_KEY); }
        set { if(value >= 1f && value <= 3f) PlayerPrefs.SetFloat(DIFFICULTY_KEY, value); }
    }
}
