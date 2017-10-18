using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour 
{
	private static Teleporter instance;

	private Transform jumpPuzzle;
	private Transform pressurePlatePuzzle;
	private Transform shadowPuzzle;
	private Transform bossFight;
    private Transform hub;
    private Transform tutEnd;
    private Transform player;
    
	private Scene scene;

	private void Awake()
	{
        if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this);

        SceneManager.sceneLoaded += SceneManager_SceneLoaded;

        SceneManager_SceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void SceneManager_SceneLoaded(Scene loadedScene, LoadSceneMode mode)
	{
		scene = loadedScene;
		if((Application.isEditor || Debug.isDebugBuild))
            FindTransforms();
    }

    private void FindTransforms()
    {
		if(scene.buildIndex != 3 && scene.buildIndex != 4)
			return;
		
		player = FindObjectOfType<Player>().transform;
		if(scene.buildIndex == 4)
		{
			pressurePlatePuzzle = GameObject.FindGameObjectWithTag ("TPplate").transform;
			jumpPuzzle = GameObject.FindGameObjectWithTag ("TPjump").transform;
			shadowPuzzle = GameObject.FindGameObjectWithTag ("TPshadow").transform;
			bossFight = GameObject.FindGameObjectWithTag ("TPboss").transform;
			hub = GameObject.FindGameObjectWithTag ("TPhub").transform;
		}
		else
			tutEnd = GameObject.FindGameObjectWithTag("TPend").transform;
    }

    private void Update()
	{
        if(!(Application.isEditor || Debug.isDebugBuild))
            return;

        if(scene.buildIndex == 3)
        {
            if(Input.GetKeyDown(KeyCode.Alpha9))
                player.position = tutEnd.position;
            if(Input.GetKeyDown(KeyCode.Alpha0))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
		else if(scene.buildIndex == 4)
        {
            if(Input.GetKeyDown(KeyCode.Alpha6))
                player.position = hub.position;
            if(Input.GetKeyDown(KeyCode.Alpha7))
                player.position = jumpPuzzle.position;
            if(Input.GetKeyDown(KeyCode.Alpha8))
                player.position = shadowPuzzle.position;
            if(Input.GetKeyDown(KeyCode.Alpha9))
                player.position = pressurePlatePuzzle.position;
            if(Input.GetKeyDown(KeyCode.Alpha0))
                player.position = bossFight.position;
        }
	}
}