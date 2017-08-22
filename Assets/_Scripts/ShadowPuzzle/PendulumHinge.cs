using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumHinge : MonoBehaviour {

	private Pendulum pen;

	public Vector3 EulerFirst;
	public Vector3 EulerSecond;

	private Vector3 TargetEuler;
	private Vector3 CurrentEuler;
	private Vector3 BufferEuler;

	private GameObject hinge;

	//currently targeting the second Euler angle?
	bool target; 

	public float degreeSwing;
	public float swingSpeedMult = 1f;
	private float currSwingSpeed;
	private float fastSwingSpeed = 1f;
	private float slowSwingSpeed = 0.1f;

	private float changeTime;
	public float changeRate = 4f;

	// Use this for initialization
	void Start () {
		
		hinge = this.gameObject;

		pen = hinge.GetComponentInChildren<Pendulum> ();

		if (degreeSwing == null) {
			degreeSwing = 30f;
		}

		EulerFirst = new Vector3 (degreeSwing / 2, 0, 0);
		EulerSecond = new Vector3 (-degreeSwing / 2, 0, 0);

		target = false;

		TargetEuler = EulerFirst;
	}
	
	// Update is called once per frame
	void Update () {
		MovePendulum ();
	}

	void MovePendulum() {

		if (ItsTime()) {
			changeTime = Time.time + changeRate;

			target = !target;
			TargetEuler = !target ? EulerFirst : EulerSecond;
		}

		CheckSlowTime ();

		if (pen.SlowTime) {
			changeTime += Time.deltaTime * (fastSwingSpeed - slowSwingSpeed);
		}

		BufferEuler = Vector3.Lerp (BufferEuler, TargetEuler, Time.deltaTime * currSwingSpeed * swingSpeedMult);
		CurrentEuler = Vector3.Lerp (CurrentEuler, BufferEuler, Time.deltaTime * currSwingSpeed * swingSpeedMult);

		/*if (NearAngles ()) {
			CurrentEuler = TargetEuler;
			BufferEuler = CurrentEuler;
		}*/

		hinge.transform.rotation = Quaternion.Euler (CurrentEuler);
	}

	bool ItsTime() {
		return Time.time > changeTime;
	}

	/*bool AtAngle() {
		return (!target && (hinge.transform.rotation.eulerAngles == EulerFirst)) || (target && (hinge.transform.rotation.eulerAngles == EulerSecond));
	}*/

	void CheckSlowTime() {
		currSwingSpeed = pen.SlowTime ? slowSwingSpeed : fastSwingSpeed;
	}

	/*bool NearAngles() {
		Vector3 checkVec;
		checkVec = TargetEuler - CurrentEuler;

		return checkVec.magnitude < 0.01f;
	}*/
}
