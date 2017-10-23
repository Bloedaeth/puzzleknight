using UnityEngine;

public static class PlayPrefs
{
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    public const float DEFAULT_MUSIC_VOLUME = 0.5f;

    public static float MusicVolume
    {
        get { return PlayerPrefs.HasKey(MUSIC_VOLUME_KEY) ? PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY) : DEFAULT_MUSIC_VOLUME; }
        set { PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, Mathf.Clamp(value, 0f, 1f)); }
    }

    private const string GAME_VOLUME_KEY = "GameVolume";

    public const float DEFAULT_GAME_VOLUME = 0.5f;

    public static float GameSoundVolume
    {
        get { return PlayerPrefs.HasKey(GAME_VOLUME_KEY) ? PlayerPrefs.GetFloat(GAME_VOLUME_KEY) : DEFAULT_GAME_VOLUME; }
        set { PlayerPrefs.SetFloat(GAME_VOLUME_KEY, Mathf.Clamp(value, 0f, 1f)); }
    }

    private const string GAME_CONTRAST_KEY = "GameContrast";

    public const float DEFAULT_GAME_CONTRAST = 0.5f;

    public static float GameContrast
    {
        get { return PlayerPrefs.HasKey(GAME_CONTRAST_KEY) ? PlayerPrefs.GetFloat(GAME_CONTRAST_KEY) : DEFAULT_GAME_CONTRAST; }
        set { PlayerPrefs.SetFloat(GAME_CONTRAST_KEY, Mathf.Clamp(value, 0f, 1f)); }
    }

    private const string POPUP_DISPLAYED_KEY = "PopupDisplayed";

    public const bool DEFAULT_POPUP_DISPLAYED = false;

    public static bool PopupDisplayed
    {
        get { return PlayerPrefs.HasKey(POPUP_DISPLAYED_KEY) ? PlayerPrefs.GetInt(POPUP_DISPLAYED_KEY) == 1 : DEFAULT_POPUP_DISPLAYED; }
        set { PlayerPrefs.SetInt(POPUP_DISPLAYED_KEY, value ? 1 : 0); }
    }

    private const string LOGGING_ENABLED_KEY = "LoggingEnabled";

    public const bool DEFAULT_LOGGING_ENABLED = true;

    public static bool LoggingEnabled
    {
        get { return PlayerPrefs.HasKey(LOGGING_ENABLED_KEY) ? PlayerPrefs.GetInt(LOGGING_ENABLED_KEY) == 1 : DEFAULT_LOGGING_ENABLED; }
        set { PlayerPrefs.SetInt(LOGGING_ENABLED_KEY, value ? 1 : 0); }
    }
}
