using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSectionDisabler : MonoBehaviour {

	GameObject[] sectionObjects;
	SectionID[] sections;
	DisablerID[] colliders;

	bool[] currStates;

	// Use this for initialization
	void Start () {
		colliders = GetComponentsInChildren<DisablerID> ();
		sections = GetComponentsInChildren<SectionID> ();
		currStates = new bool[colliders.Length];

		sectionObjects = new GameObject[sections.Length];

		for (int i = 0; i < sections.Length; i++) {
			sectionObjects [i] = sections [i].gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (SomethingWrong ()) {

		}
	}

	bool SomethingWrong() {
		for (int i = 0; i < currStates.Length; i++) {
			if (currStates [i] != colliders [i].inside) {
				return true;
			}
			return false;
	}
}
