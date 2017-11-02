using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSectionDisabler : MonoBehaviour {

	GameObject[] sectionObjects;
	SectionID[] sections;
	DisablerID[] colliders;

	bool[] currStates;

	bool defaultsApplied = false;

	void OnEnable() {
		defaultsApplied = false;
	}

	// Use this for initialization
	void Start () {
		colliders = GetComponentsInChildren<DisablerID> ();
		sections = GetComponentsInChildren<SectionID> ();
		currStates = new bool[colliders.Length];



		sectionObjects = new GameObject[sections.Length];

		for (int i = 0; i < sections.Length; i++) {
			sectionObjects [i] = sections [i].gameObject;
		}

		SortSections ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!defaultsApplied && Time.time > 0.1f) {
			defaultsApplied = true;

			for (int i = 0; i < currStates.Length; i++) {
				currStates [i] = true;
			}
		}

		CheckColliders ();
	}

	void SortSections() {
		SectionID[] shadow = new SectionID[sections.Length];

		for (int i = 0; i < shadow.Length; i++) {
			foreach (SectionID temp in sections) {
				if (temp.thisSectionType == colliders [i].colliderType) {
					shadow[i] = temp;
					break;
				}
			}
		}

		sections = shadow;
	}

	void CheckColliders() {
		for (int i = 0; i < currStates.Length; i++) {
			if (currStates [i] != colliders [i].inside) {
				currStates [i] = colliders [i].inside;

				UpdateSection (i);

			}
		}
	}

	void UpdateSection(int section) {
		sections [section].gameObject.SetActive (currStates [section]);
	}
}
