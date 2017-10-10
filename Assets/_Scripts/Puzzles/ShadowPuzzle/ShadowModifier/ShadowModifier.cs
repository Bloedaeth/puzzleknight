using GameLogging;
using UnityEngine;

public class ShadowModifier : MonoBehaviour {

	public bool inlight;

	public GameObject shadowChecker;

	public LightStore ls;
	protected Light[] lights;

	protected MeshFilter mf;
	protected MeshRenderer mr;
	protected SkinnedMeshRenderer smr;

	protected Mesh m;
	protected Vector3[] MeshVertices;
	protected Vector3[][] CheckVertices;

	public ParticleSystem particle;
	private bool emitted;

	// Use this for initialization
	protected void Start () {
		if (smr == null) {
			smr = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
		}

		if (mr == null) {
			mr = this.gameObject.GetComponentInChildren<MeshRenderer> ();
		}

		if (ls == null) { // Last resort, Ls SHOULD NOT be null
			ls = GameObject.Find ("LightStore").GetComponent<LightStore> ();
		}

		if (particle == null) {
			particle = this.gameObject.GetComponentInChildren<ParticleSystem> ();
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
	protected void Update () {

		if (lights == null || ls.UpdateLights()) {
			lights = ls.GetLights ();
			return;
		}
		inlight = InLight ();

		UpdateModels (!inlight);
	}

	protected void UpdateModels(bool visible) {
		if (visible)
        {
            BuildDebug.Log("Setting ShadowCastingMode to On for " + name, true);
            if (smr != null) {
				smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			} else if (mr != null) {
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			}

			emitted = false;
		}
        else
        {
            BuildDebug.Log("Setting ShadowCastingMode to ShadowsOnly for " + name, true);
            if (smr != null) {
				smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			} else if (mr != null) {
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			}
			if (!emitted) {
				if (particle == null) {
					ResetParticleSystem ();
				} else {
					particle.Play ();
				}
				emitted = true;
			}
		}
	}

	// Calculate the vertices that are going to be checked, by calculating the average distance away from a light, and then returning an array of the vertex points that correspond to each light.

	protected Vector3[][] CalculateVertices(Vector3[] vertices) {
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

	protected float AverageMagnitude (Vector3 LtoO, Vector3[] vertices) {
		float sumOfMags = 0;

		if (vertices.Length < 50) {
			for (int i = 0; i < vertices.Length; i++) {
				sumOfMags += (LtoO + vertices [i]).magnitude;
			}
			sumOfMags /= (float)vertices.Length;
		} else {
			return float.MaxValue;
		}
		return float.MaxValue;//sumOfMags;
	}

	protected virtual bool InLight () {
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
					return true;
				}
			}
		}
		return false;
	}

	public void ResetParticleSystem() {
		particle = GetComponentInChildren<ParticleSystem> ();
	}

	public bool HasParticleSystem() {
		return particle != null;
	}
}
