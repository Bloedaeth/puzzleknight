using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour {

	bool active;

	Transform ChaseObject;
	Vector3 coOrigPos;
	Quaternion coOrigRot;

	Vector3 pointDir;
	Vector3 positionBuffer;

	Quaternion pointRot;
	Quaternion rotationBuffer;

	TransformIterator ChasePoints;
	public int currPoint { get { return ChasePoints.GetIndex (); } }

	Camera cam;

	bool hanging;
	float hangTime = 2f;
	float currHangTime = 0f;

	float moveSpeed = 3f;

	UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl tpuc;
	Player p;

	// Use this for initialization
	void Start () {
		GetChasePointsAndObject ();
		cam = Camera.main;
		tpuc = FindObjectOfType<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ();
		p = FindObjectOfType<Player> ();
	}

	void GetChasePointsAndObject () {
		ChaseObject = GetComponentInChildren<CameraChaseObject> ().t;

		if (!ChaseObject) {
			Debug.LogError ("No chase object in " + name + " group.");
		}

		CameraChasePoint[] ccp = GetComponentsInChildren<CameraChasePoint> ();

		if (ccp.Length < 1) {
			Debug.LogError ("No chase points in " + name + " group.");
		}

		Transform [] points = new Transform[ccp.Length];

		for (int i = 0; i < ccp.Length; i++) {
			points [i] = ccp [i].t;
		}

		ChasePoints = new TransformIterator (points);
	}

	// Update is called once per frame
	void Update () {
		if (active) {
			MoveCamera ();
		} 
	}

	void MoveCamera() {
		positionBuffer = Vector3.Lerp (positionBuffer, pointDir, Time.deltaTime * moveSpeed);
		ChaseObject.position = Vector3.Lerp (ChaseObject.position, positionBuffer, Time.deltaTime * moveSpeed/2);

		rotationBuffer = Quaternion.Slerp (rotationBuffer, pointRot, Time.deltaTime * moveSpeed);
		tpuc.freeLookCamera.transform.rotation = Quaternion.Slerp (tpuc.freeLookCamera.transform.rotation, rotationBuffer, Time.deltaTime * moveSpeed/2);

		tpuc.freeLookCamera.camPivot.transform.localRotation = Quaternion.Slerp (tpuc.freeLookCamera.camPivot.transform.localRotation, new Quaternion (0, 0, 0, 1), Time.deltaTime);

		/*if (ChasePoints.EndList ()) {
			EndChase ();
			return;
		}*/

		if ((-pointDir + positionBuffer).magnitude < 0.1f) {
			Transform temp;
			if (ChasePoints.GetNextPoint (out temp)) {
				pointDir = temp.position;
				pointRot = temp.rotation;
			} else {
				EndChase ();
			}

		}




	}

	public void BeginChase() {
		pointDir = tpuc.freeLookCamera.camObject.transform.position;
		pointRot = tpuc.freeLookCamera.camObject.transform.rotation;

		positionBuffer = pointDir;
		rotationBuffer = pointRot;

		ChaseObject.position = pointDir;


		active = true;
		tpuc.isLooking = true;
		tpuc.freeLookCamera.orbitActive = false;
		tpuc.freeLookCamera.ToggleCamClip (true);
		tpuc.freeLookCamera.SetTarget (ChaseObject);
		tpuc.SetCameraToZeros ();
		currHangTime = 0f;

		ChasePoints.ResetPoints ();
		pointDir = ChasePoints.GetNextPoint().position;

		p.StopMovement (true);
	}

	public void EndChase() {

		hanging = true;


		while (currHangTime < hangTime) {
			currHangTime += Time.deltaTime;
			return;
		}

		active = false;
		tpuc.isLooking = false;
		tpuc.freeLookCamera.orbitActive = true;
		tpuc.freeLookCamera.ToggleCamClip (false);
		tpuc.ResetCamera(1238709123); //Very specific number, many importants.
		ResetCameraObject ();
		p.StopMovement (false);

		hanging = false;
	}

	void ResetCameraObject() {
		ChaseObject.position = coOrigPos;
		ChaseObject.rotation = coOrigRot;
	}
}