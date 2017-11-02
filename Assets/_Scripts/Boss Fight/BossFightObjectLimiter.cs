using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to disable active objects upon entry into the boss fight.
/// </summary>

public class BossFightObjectLimiter : MonoBehaviour {

	Player p;
	public GameObject[] sections;

	bool limiting;

	// Use this for initialization
	void Start () {
		if (sections.Length == 0) {
			Debug.LogError ("Please set the Puzzle sections as the objects in this array;\n and any 'section' you want to disable when the boss fight starts.");
		}

		p = FindObjectOfType<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (p.InBossFight && !limiting) {

			SetAllGameObjectsTo (limiting);

			limiting = true;

		} else if (!p.InBossFight && limiting) {
			SetAllGameObjectsTo (limiting);

			limiting = false;
		}
	}
		

	void SetAllGameObjectsTo(bool state) {
		foreach (GameObject g in sections) {
			g.SetActive (state);
		}
	}
}
