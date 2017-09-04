using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    private Slider volumeSlider;
    private MusicPlayer musicPlayer;

    void Awake()
    {
        volumeSlider = FindObjectOfType<Slider>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    void Start()
    {
        volumeSlider.value = PlayPrefs.Volume;
    }

    public void DefaultSettings()
    {
        volumeSlider.value = PlayPrefs.DEFAULT_VOLUME;
        PlayPrefs.Volume = volumeSlider.value;
    }

    public void SetVolume()
    {
        PlayPrefs.Volume = volumeSlider.value;
        musicPlayer.GetComponent<AudioSource>().volume = PlayPrefs.Volume;
    }
}
