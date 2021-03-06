﻿using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioClip[] musicArray;
    
    private Player player;
    private new AudioSource audio;
	private AudioClip levelMusic;

    private static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        audio = GetComponent<AudioSource>();
        audio.volume = PlayPrefs.MusicVolume;
        audio.clip = musicArray[0];
        audio.loop = true;
        audio.Play();
    }

    private void Update()
    {
        if(!player)
            return;
		
        if(audio.clip != musicArray[1] && player.InBossFight)
            PlayMusicAtIndex(1);
        else if(audio.clip == musicArray[1] && !player.InBossFight)
            PlayMusicAtIndex(3);
    }

    public void SetMusicVolume(float val)
    {
        audio.volume = val;
    }

    public void SetGameSoundVolume(float val)
    {
        foreach(AudioSource obj in FindObjectsOfType<AudioSource>())
        {
            if(obj.gameObject == gameObject)
                continue;

            obj.volume = val;
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicVolume(PlayPrefs.MusicVolume);
        SetGameSoundVolume(PlayPrefs.GameSoundVolume);
        GameObject mainLight = GameObject.FindGameObjectWithTag("MainLight");
        if(mainLight)
            mainLight.GetComponent<Light>().color = new Color(PlayPrefs.GameContrast, PlayPrefs.GameContrast, PlayPrefs.GameContrast, 1f);

        player = FindObjectOfType<Player>();


		if(audio.clip != musicArray[2] && scene.name.Contains("Tutorial"))
			PlayMusicAtIndex(2);
		if(audio.clip != musicArray[3] && scene.name.Contains("Deliverable"))
            PlayMusicAtIndex(3);
        else if(audio.clip != musicArray[0] && !player)
            PlayMusicAtIndex(0);
    }

    private void PlayMusicAtIndex(int index)
    {
        audio.clip = musicArray[index];
        audio.loop = true;
        audio.Play();
    }
}
