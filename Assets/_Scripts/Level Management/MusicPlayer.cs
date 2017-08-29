using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicArray;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().volume = PlayerPrefs.HasKey("Volume") ? PlayPrefs.Volume : 0.5f;
    }

    void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name.Contains("Level"))
        {
            GetComponent<AudioSource>().clip = musicArray[3];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        else if(GetComponent<AudioSource>().clip != musicArray[1] && (SceneManager.GetActiveScene().name.Contains("Start")
                || SceneManager.GetActiveScene().name.Contains("Options") || SceneManager.GetActiveScene().name.Contains("Controls")))
        {
            GetComponent<AudioSource>().clip = musicArray[1];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        else if(SceneManager.GetActiveScene().name.Contains("Win"))
        {
            GetComponent<AudioSource>().clip = musicArray[3];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        else if(SceneManager.GetActiveScene().name.Contains("Lose"))
        {
            GetComponent<AudioSource>().clip = musicArray[2];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
    }
}
