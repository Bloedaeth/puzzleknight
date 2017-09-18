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
	private Transform player;

    private bool inEditor = false;

	private void Awake()
	{
#if UNITY_EDITOR
        inEditor = true;
#endif
        if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this);

        OnLevelWasLoaded();
    }

    private void OnLevelWasLoaded()
	{
        if((inEditor || Debug.isDebugBuild) && SceneManager.GetActiveScene().name.Contains("Deliverable"))
            FindTransforms();
    }

    private void FindTransforms()
    {
        player = FindObjectOfType<Player>().transform;
        pressurePlatePuzzle = GameObject.FindGameObjectWithTag("TPplate").transform;
        jumpPuzzle = GameObject.FindGameObjectWithTag("TPjump").transform;
        shadowPuzzle = GameObject.FindGameObjectWithTag("TPshadow").transform;
        bossFight = GameObject.FindGameObjectWithTag("TPboss").transform;
        hub = GameObject.FindGameObjectWithTag("TPhub").transform;

    }

    private void Update()
	{
        if(!(inEditor || Debug.isDebugBuild))
            return;

        if(SceneManager.GetActiveScene().buildIndex == 3 && Input.GetKeyDown(KeyCode.Alpha0))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		else
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