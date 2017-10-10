using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public float loadLevelAfter;

    private static LevelManager instance;

    private GameObject loadingScreen;
    private Slider progressSlider;
    private Text progressText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(SceneManager.GetActiveScene().name == "Splash Screen")
            Invoke("LoadNextLevel", loadLevelAfter);
    }

    private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadingScreen[] objs = Resources.FindObjectsOfTypeAll<LoadingScreen>();
        if(objs.Length > 0)
            loadingScreen = objs[0].gameObject;
        if(loadingScreen)
        {
            progressSlider = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<ProgressText>().GetComponent<Text>();
        }
        else
        {
            loadingScreen = null;
            progressSlider = null;
            progressText = null;
        }
    }

    public void LoadLevel(string levelName) { SceneManager.LoadScene(levelName); }
    public void LoadLevel(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
    public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    public void BtnLoadLevelAsync(string levelName) { FindObjectOfType<LevelManager>().LoadLevelAsync(levelName); }
    public void BtnLoadLevelAsync(int sceneIndex) { FindObjectOfType<LevelManager>().LoadLevelAsync(sceneIndex); }

    public void LoadLevelAsync(string levelName)
    {
        FindObjectOfType<SoundManager>().SetMusicVolume(0);
        FindObjectOfType<SoundManager>().SetGameSoundVolume(0);
        StartCoroutine(LoadAsynchronously(levelName));
    }

    public void LoadLevelAsync(int sceneIndex)
    {
        FindObjectOfType<SoundManager>().SetMusicVolume(0);
        FindObjectOfType<SoundManager>().SetGameSoundVolume(0);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadNextLevelAsync()
    {
        FindObjectOfType<SoundManager>().SetMusicVolume(0);
        FindObjectOfType<SoundManager>().SetGameSoundVolume(0);
        LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame() { Application.Quit(); }

    private IEnumerator LoadAsynchronously(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            progressSlider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            progressSlider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
