using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Dumb class used to identify the Disabler colliders.
/// </summary>
public class DisablerID : MonoBehaviour {
	public SectionID.sectionType colliderType;

	public bool inside;

	void OnTriggerStay(Collider o) { 
		if (!inside && o.GetComponent<Player> ()) {
			inside = true;
		}
	}

	void OnTriggerExit(Collider o) { 
		if (o.GetComponent<Player> ()) {
			inside = false;
		}
	}
}
