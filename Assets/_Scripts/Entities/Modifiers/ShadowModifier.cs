using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowModifier : MonoBehaviour {

	public bool vulnerable;

	private MeshRenderer mr;

	private Mesh m;
	private Vector3[] MeshVertices;
	private Vector3[][] CheckVertices;

	public LightStore ls;
	private Light[] lights;

	// Use this for initialization
	void Start () {
		mr = this.gameObject.GetComponent<MeshRenderer> ();
		m = this.gameObject.GetComponent<MeshFilter>().mesh;
		MeshVertices = m.vertices;
		lights = ls.GetLights ();

	}
	
	// Update is called once per frame
	void Update () {
		if (lights == null) {
			lights = ls.GetLights ();
			return;
		}
		//print ("Oh boi, here we go!");
		vulnerable = !InLight ();

		if (vulnerable) {
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		} else {
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		}
	}

	// Calculate the vertices that are going to be checked, by calculating the average distance away from a light, and then returning an array of the vertex points that correspond to each light.

	private Vector3[][] CalculateVertices(Vector3[] vertices) {
		//print ("Open Calculate Vertices");
		Vector3[][] v = new Vector3[lights.Length][];
	
		//print ("Enter Light Loop");
		for (int i = 0; i < lights.Length; i++) {
			//print ("Light number: " + i.ToString());
			Vector3 LightVec = lights [i].transform.position;
			Vector3 LtoObject = -(LightVec - transform.position);
			//print ("Light Vector = " + LightVec.ToString());
			//print ("Light To Object Vector = " + LtoObject.ToString());

			//print ("Initialise Boolean Array: ");
			bool[] boolArr = new bool[vertices.GetLength(0)];

			//find average
			//print("Find Average");
			float aveMag = AverageMagnitude (LtoObject, vertices);
			//print ("Start count:");
			int count = 0;

			//print ("Enter for loop to set booleans");
			for (int j = 0; j < vertices.Length; j++) {

				if ((LtoObject + vertices [j]).magnitude <= aveMag) {
					//print ("is less than average");
					//print ("Vertex is: " + vertices [j].ToString ());
					//print ("Vector is: " + (LtoObject + vertices [j]).ToString ());
					//print ("Magnitude is: " + (LtoObject + vertices [j]).magnitude);
					//print ("Average = " + aveMag.ToString ());
					boolArr [j] = true;
					count++;
					//print (count.ToString ());
				} //else {print ("is not less than average");}
				//print ("For current loop:\nj = "+j.ToString()+"\ncount = "+count.ToString()+"\nboolarr = " + boolArr[j].ToString());
			}

			//print ("Set up check vertices variable.");
			Vector3[] cVert = new Vector3[count];
			//print ("cVert Length = " + cVert.Length.ToString ());
			count = 0;
			//print ("Enter for loop to set checkable vertices.");
			for (int j = 0; j < vertices.Length; j++) {
				//print ("For vertex point: " + j.ToString ());
				//print ("At local position: " + vertices[j].ToString());
				//print ("Will it be added? " + boolArr[j].ToString());


				if (boolArr [j]) {
					//print ("Adding vertex at position: " + j.ToString () + "\nTo cVert at position: " + (count + 1).ToString () + "/" + cVert.Length.ToString ());
					cVert [count] = vertices [j];
					count++;
					//if (i == 0) Debug.DrawLine(transform.position,transform.position + (vertices[j] * 2), Color.white);
				} else {
					//if (i == 0) Debug.DrawLine(transform.position,transform.position + (vertices[j] * 2), Color.grey);
				}
			}

			//print ("Set checkable vertices for light " + i.ToString());
			v [i] = cVert;
			//print ("End iteration: " + i.ToString ());
		}
		//print ("End loop. \nReturn array.");
		return v;
	}

	private float AverageMagnitude (Vector3 LtoO, Vector3[] vertices) {
		//print ("Enter Average Magnitude...");
		float sumOfMags = 0;

		//print ("Enter For Loop: max I = " + vertices.Length.ToString ());
		for (int i = 0; i < vertices.Length; i++) {
			//print ("i = " + i.ToString ());
			sumOfMags += (LtoO + vertices [i]).magnitude;
			//print ("new Sum of Magnitudes: " + sumOfMags.ToString ());
		}
		//print ("Exit for loop; total Mags = " + sumOfMags.ToString());
		sumOfMags /= (float) vertices.Length;
		//print ("Average mags = " + sumOfMags.ToString());

		//print ("Exit average magnitude");
		return sumOfMags;
	}

	private bool InLight () {
		//print ("Enter InLight");

		//print ("Check Vertices pls");
		CheckVertices = CalculateVertices (MeshVertices);
		//print ("Vertices is now: " + CheckVertices.ToString ());

		for (int i = 0; i < lights.Length; i++) {
			for (int j = 0; j < CheckVertices[i].Length; j++) {
				
				Ray r = new Ray (transform.position + (CheckVertices[i][j] * transform.localScale.x), 
					(lights [i].transform.position - transform.position) - (CheckVertices[i][j] * transform.localScale.x));

				if (!Physics.Raycast (r, (r.origin - lights [i].transform.position).magnitude)) {
					//if (i == 0) Debug.DrawRay (transform.position + (CheckVertices[i][j] * transform.localScale.x), (lights [i].transform.position - transform.position) - (CheckVertices[i][j] * transform.localScale.x), Color.green);
					return true;
				} else {
					//if (i == 0) Debug.DrawRay (transform.position + (CheckVertices[i][j] * transform.localScale.x), (lights [i].transform.position - transform.position) - (CheckVertices[i][j] * transform.localScale.x), Color.red);
				}
			}
		}
		return false;
	}
}
