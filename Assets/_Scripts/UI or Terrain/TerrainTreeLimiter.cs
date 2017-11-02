using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTreeLimiter : MonoBehaviour {

	Terrain t;
	Player p;

	float[] treeDistances = { 40f, 100f };

	// Use this for initialization
	void Start () {
		t = GetComponent<Terrain> ();
		p = GameObject.FindObjectOfType<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (p.InBossFight && t.treeDistance == treeDistances [1]) {
			t.treeDistance = treeDistances [0];
		} else if (!p.InBossFight && t.treeDistance == treeDistances [0]) {
			t.treeDistance = treeDistances [1];
		}
	}
}
