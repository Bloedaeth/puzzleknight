﻿using UnityEngine;
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

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this);

        if(!Debug.isDebugBuild)
            FindTransforms();
        else
#if UNITY_EDITOR
            FindTransforms();
#endif
    }

    private void OnLevelWasLoaded()
	{
        if(!Debug.isDebugBuild)
            FindTransforms();
        else
#if UNITY_EDITOR
            FindTransforms();
#endif
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
        if(!Debug.isDebugBuild)
            return;
        else
#if UNITY_EDITOR
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
#endif
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