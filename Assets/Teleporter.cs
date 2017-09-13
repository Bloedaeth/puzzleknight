using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour 
{
	private static Teleporter instance;

	private Transform jumpPuzzle;
	private Transform pressurePlatePuzzle;
	private Transform shadowPuzzle;
	private Transform bossFight;

	private Transform player;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			GameObject.DontDestroyOnLoad();
		}
		else
			Destroy (this);
	}

	private void OnLevelWasLoaded()
	{
		player = FindOjbectOfType<player> ().transform;
		pressurePlatePuzzle = FindObjectWithTag("TPplate").transform;
		jumpPuzzle = FindObjectWithTag("TPjump").transform;
		shadowPuzzle = FindObjectWithTag("TPshadow").transform;
		bossFight = FindObjectWithTag("TPboss").transform;
	}

	private void Update()
	{
		if(SceneManager.GetActiveScene().buildIndex == 3 && Input.GetKeyDown(KeyCode.Alpha0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		else
		{
			if (Input.GetKeyDown (KeyCode.Alpha7)) {
				player.position = jumpPuzzle.position;
			}
			if (Input.GetKeyDown (KeyCode.Alpha8)) {
				player.position = shadowPuzzle.position;
			}
			if (Input.GetKeyDown (KeyCode.Alpha9)) {
				player.position = pressurePlatePuzzle.position;
			}
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				player.position = bossFight.position;
			}
		}
	}
}