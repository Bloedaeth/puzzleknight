using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float loadLevelAfter;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Splash Screen")
            Invoke("LoadNextLevel", loadLevelAfter);
    }

    public void LoadLevel(string levelName) { SceneManager.LoadScene(levelName); }

    public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    public void QuitGame() { Application.Quit(); }
}
