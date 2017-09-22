using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    private Slider musicVolumeSlider;
    private Slider gameSoundVolumeSlider;
    private Slider gameContrastSlider;

    private SoundManager musicPlayer;

    private static OptionsScript instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        musicPlayer = FindObjectOfType<SoundManager>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string name = scene.name;
        if(name.Contains("Options") || name.Contains("Tutorial") || name.Contains("Deliverable"))
            FindAll<Slider>(true);

        //Debug.Log("Hmm");
        //musicVolumeSlider.value = PlayPrefs.MusicVolume;
        //Debug.Log(musicVolumeSlider);
        //gameSoundVolumeSlider.value = PlayPrefs.GameSoundVolume;
        //Debug.Log(gameSoundVolumeSlider);
        //gameContrastSlider.value = PlayPrefs.GameContrast;
        //Debug.Log(gameContrastSlider);
    }

    private void FindAll<T>(bool findInactive)
    {
        GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
        
        foreach(GameObject obj in objs)
        {
            Slider[] sliders = obj.GetComponentsInChildren<Slider>(findInactive);
            Debug.Log(sliders.Length);
            if(sliders.Length > 0)
            {
                Debug.Log("getting sliders");
                musicVolumeSlider = sliders.First(s => s.CompareTag("MusicVolume"));
                gameSoundVolumeSlider = sliders.First(s => s.CompareTag("GameSoundVolume"));
                gameContrastSlider = sliders.First(s => s.CompareTag("GameContrast"));
                Debug.Log(musicVolumeSlider);
                Debug.Log(gameContrastSlider);
                Debug.Log(gameSoundVolumeSlider);
            }
        }
    }

    public void DefaultSettings()
    {
        musicVolumeSlider.value = PlayPrefs.DEFAULT_MUSIC_VOLUME;
        gameSoundVolumeSlider.value = PlayPrefs.DEFAULT_GAME_VOLUME;
        gameContrastSlider.value = PlayPrefs.DEFAULT_GAME_CONTRAST;
    }

    public void SetMusicVolume()
    {
        Debug.Log(musicVolumeSlider);
        PlayPrefs.MusicVolume = musicVolumeSlider.value;
        musicPlayer.GetComponent<AudioSource>().volume = PlayPrefs.MusicVolume;
    }

    public void SetGameVolume()
    {
        PlayPrefs.GameSoundVolume = gameSoundVolumeSlider.value;
        musicPlayer.SetGameVolume(PlayPrefs.GameSoundVolume);
    }

    public void SetContrast()
    {
        PlayPrefs.GameContrast = gameContrastSlider.value;
        //Set contrast
    }
}
