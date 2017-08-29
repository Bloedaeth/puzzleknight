using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    private Slider volumeSlider;
    private Slider difficultySlider;
    private MusicPlayer musicPlayer;

    private bool hasVol;
    private bool hasDif;

    private const float DEFAULT_VOLUME = 0.5f;
    private const float DEFAULT_DIFFICULTY = 2f;

    void Awake()
    {
        hasVol = PlayerPrefs.HasKey("Volume");
        hasDif = PlayerPrefs.HasKey("Difficulty");

        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach(Slider slider in sliders)
        {
            if(slider.name == "Volume Slider")
                volumeSlider = slider;
            else
                difficultySlider = slider;
        }
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    void Start()
    {
        volumeSlider.value = hasVol ? PlayPrefs.Volume : DEFAULT_VOLUME;
        difficultySlider.value = hasDif ? PlayPrefs.Difficulty : DEFAULT_DIFFICULTY;
    }

    public void DefaultSettings()
    {
        volumeSlider.value = DEFAULT_VOLUME;
        difficultySlider.value = DEFAULT_DIFFICULTY;
        PlayPrefs.Volume = volumeSlider.value;
        PlayPrefs.Difficulty = difficultySlider.value;
    }

    public void SetVolume()
    {
        PlayPrefs.Volume = volumeSlider.value;
        musicPlayer.GetComponent<AudioSource>().volume = PlayPrefs.Volume;
    }

    public void SetDifficulty()
    {
        PlayPrefs.Difficulty = difficultySlider.value;
    }
}
