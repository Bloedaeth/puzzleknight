using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowModifier : MonoBehaviour {

	public bool vulnerable;

	public GameObject shadowChecker;

	public LightStore ls;
	private Light[] lights;

	public MeshFilter mf;
	public MeshRenderer mr;
	public SkinnedMeshRenderer smr;

	private Mesh m;
	private Vector3[] MeshVertices;
	private Vector3[][] CheckVertices;

	// Use this for initialization
	void Start () {
		if (smr == null) {
			smr = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
		}
		if (mr == null) {
			mr = this.gameObject.GetComponentInChildren<MeshRenderer> ();
		}

		if (ls == null) { // Last resort, Ls SHOULD NOT be null
			ls = GameObject.Find ("LightStore").GetComponent<LightStore> ();
		}

		m = new Mesh ();
		if (shadowChecker != null) {
			m = shadowChecker.GetComponent<MeshFilter> ().mesh;
		} else {
			if (mf != null) {
				m = mf.mesh;
			} else {
				m = this.gameObject.GetComponentInChildren<MeshFilter> ().mesh;
			}
		}

		MeshVertices = m.vertices;
		lights = ls.GetLights ();

	}
	
	// Update is called once per frame
	void Update () {

		if (lights == null || ls.UpdateLights()) {
			lights = ls.GetLights ();
			return;
		}
		vulnerable = !InLight ();

		if (vulnerable) {
			if (smr != null) {
				smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			} else if (mr != null) {
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			}
		} else {
			if (smr != null) {
				smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			} else if (mr != null) {
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			}
		}
	}

	// Calculate the vertices that are going to be checked, by calculating the average distance away from a light, and then returning an array of the vertex points that correspond to each light.

	private Vector3[][] CalculateVertices(Vector3[] vertices) {
		Vector3[][] v = new Vector3[lights.Length][];
	
		for (int i = 0; i < lights.Length; i++) {
			Vector3 LightVec = lights [i].transform.position;
			Vector3 LtoObject = -(LightVec - transform.position);

			bool[] boolArr = new bool[vertices.GetLength(0)];

			float aveMag = AverageMagnitude (LtoObject, vertices);
			int count = 0;

			for (int j = 0; j < vertices.Length; j++) {

				if ((LtoObject + vertices [j]).magnitude <= aveMag) {
					boolArr [j] = true;
					count++;
				}
			}
				
			Vector3[] cVert = new Vector3[count];
			count = 0;

			for (int j = 0; j < vertices.Length; j++) {
				if (boolArr [j]) {
					cVert [count] = vertices [j];
					count++;
				}
			}
			v [i] = cVert;
		}
		return v;
	}

	private float AverageMagnitude (Vector3 LtoO, Vector3[] vertices) {
		float sumOfMags = 0;

		for (int i = 0; i < vertices.Length; i++) {
			sumOfMags += (LtoO + vertices [i]).magnitude;
		}
		sumOfMags /= (float) vertices.Length;

		return sumOfMags;
	}

	private bool InLight () {
		CheckVertices = CalculateVertices (MeshVertices);

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
					return true;
				} else {
					Debug.DrawRay (transform.position + (CheckVertices [i] [j]), 
						(lights [i].transform.position - transform.position) - (CheckVertices [i] [j]), Color.red);
				}
			}
		}
		return false;
	}
}
