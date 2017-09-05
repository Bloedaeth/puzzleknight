using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicArray;

    private Player player;
    private new AudioSource audio;

    private static MusicPlayer instance;

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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        audio = GetComponent<AudioSource>();
        audio.volume = PlayPrefs.Volume;
        audio.clip = musicArray[0];
        audio.loop = true;
        audio.Play();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<Player>();
        string sceneName = scene.name;
        if(audio.clip != musicArray[0] && (sceneName.Contains("Start") || sceneName.Contains("Options") || sceneName.Contains("Controls")))
            PlayMusicAtIndex(0);
        else if(player)
        {
            if(audio.clip != musicArray[2] && player.InBossFight)
                PlayMusicAtIndex(2);
            else if(audio.clip != musicArray[1])
                PlayMusicAtIndex(1);
        }
    }

    private void PlayMusicAtIndex(int index)
    {
        audio.clip = musicArray[index];
        audio.loop = true;
        audio.Play();
    }
}
