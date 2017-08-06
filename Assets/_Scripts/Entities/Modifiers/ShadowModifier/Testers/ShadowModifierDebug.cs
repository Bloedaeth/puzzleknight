using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowModifierDebug : ShadowModifier {

	protected override bool InLight () {
		CheckVertices = CalculateVertices (MeshVertices);
		bool b = false;

		for (int i = 0; i < lights.Length; i++) {
			for (int j = 0; j < CheckVertices[i].Length; j++) {
				if (shadowChecker != null) {
					CheckVertices [i] [j].Scale (shadowChecker.transform.localScale);
					CheckVertices [i] [j] += shadowChecker.transform.localPosition;
				} else {
					CheckVertices [i] [j].Scale (transform.localScale);
				}

				Ray r = new Ray (transform.position + (CheckVertices[i][j]), 
					(lights [i].transform.position - transform.position) - (CheckVertices [i] [j]));

				if (!Physics.Raycast (r, (r.origin - lights [i].transform.position).magnitude)) {
					Debug.DrawRay (transform.position + (CheckVertices [i] [j]), 
						(lights [i].transform.position - transform.position) - (CheckVertices [i] [j]), Color.green);
					b = true;
				} else {
					Debug.DrawRay (transform.position + (CheckVertices [i] [j]), 
						(lights [i].transform.position - transform.position) - (CheckVertices [i] [j]), Color.red);
				}
			}
		}
		return b;
	}
}
