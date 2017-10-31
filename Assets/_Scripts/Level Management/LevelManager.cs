using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public float loadLevelAfter;

    private static LevelManager instance;

    private AsyncOperation operation;
    [SerializeField] private GameObject loadingScreen;
    private Slider progressSlider;
    private Text progressText;
    private GameObject anyKeyToContinue;

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
        LoadingScreen[] objs = Resources.FindObjectsOfTypeAll<LoadingScreen>().Where(o => o.hideFlags != HideFlags.HideInHierarchy).ToArray();
        if(objs.Length > 0)
            loadingScreen = objs[0].gameObject;
        if(loadingScreen)
        {
            progressSlider = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<ProgressText>().GetComponent<Text>();
            anyKeyToContinue = loadingScreen.GetComponentInChildren<FlashingImage>(true).gameObject;
        }
        else
        {
            loadingScreen = null;
            progressSlider = null;
            progressText = null;
            anyKeyToContinue = null;
        }
    }

    public void LoadLevel(string levelName) { SceneManager.LoadScene(levelName); }
    public void LoadLevel(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
    public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    public void BtnLoadLevelAsync(string levelName) { FindObjectOfType<LevelManager>().LoadLevelAsync(levelName); }
    public void BtnLoadLevelAsync(int sceneIndex) { FindObjectOfType<LevelManager>().LoadLevelAsync(sceneIndex); }

    public void ReturnToMenu(string menu)
    {
        Time.timeScale = 1;
        LoadLevel(menu);
    }

    public void LoadLevelAsync(string levelName)
    {
        SilenceSounds();
        operation = SceneManager.LoadSceneAsync(levelName);
        StartCoroutine(UpdateSlider());
    }

    public void LoadLevelAsync(int sceneIndex)
    {
        SilenceSounds();
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        StartCoroutine(UpdateSlider());
    }

    public void LoadNextLevelAsync()
    {
        LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame() { Application.Quit(); }

    public void ContinueToScene()
    {
        operation.allowSceneActivation = true;
    }

    private void SilenceSounds()
    {
        SoundManager sm = FindObjectOfType<SoundManager>();
        sm.SetMusicVolume(0);
        sm.SetGameSoundVolume(0);
    }

    private IEnumerator UpdateSlider()
    {
        progressSlider.value = progressSlider.minValue;
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        while(progressSlider.value < progressSlider.maxValue)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            progressSlider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return new WaitForEndOfFrame();
        }

        anyKeyToContinue.SetActive(true);
        yield return null;
    }
}
