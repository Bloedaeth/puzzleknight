using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowCanvasScript : MonoBehaviour {

	public Text t;
	int EntityCount;
	int EnemyCount;

	float FPSrefreshRate = 1f;
	float FPSrefreshTime;
	float FPS;

	// Use this for initialization
	void Awake () {

		t = GetComponentInChildren<Text> ();

		EntityCount = 1;
		EnemyCount = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Q)) {
			EntityCount++;
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			EnemyCount++;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			EntityCount = 0;
			EnemyCount = 0;
		}

		if (Time.time > FPSrefreshTime) {
			FPS = 1 / Time.deltaTime;
			FPSrefreshTime = Time.time + FPSrefreshRate;
		}

		t.text = "Entity Count: " + EntityCount.ToString () + "\nEnemy Count: " + EnemyCount.ToString () + "\nTotal Count: " + (EnemyCount + EntityCount).ToString () + "\nFPS: " + FPS;
	}
}
