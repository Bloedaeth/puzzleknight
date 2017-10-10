using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    //Load Level Components
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    //


    public float loadLevelAfter;

    private static LevelManager instance;

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
        if(SceneManager.GetActiveScene().name == "Splash Screen")
            Invoke("LoadNextLevel", loadLevelAfter);
    }

    public void LoadLevel(string levelName) { SceneManager.LoadScene(levelName); }

    public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    public void QuitGame() { Application.Quit(); }

    //Load Level Portion
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
