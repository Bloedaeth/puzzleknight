using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dumb section identifier code.
/// </summary>
public class SectionID : MonoBehaviour {

	public enum sectionType {
		Hub,
		Door,
		Jump,
		Shadow,
		Plate
	}

	public sectionType thisSectionType;
}
