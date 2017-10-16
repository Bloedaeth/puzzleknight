using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Percent as Time % swingRate / swingRate, will give a number 0 - 1, which resets back to 0 once 1 is achieved.
// Function has to be -0.5 * cos(2 * pi * stage) + 0.5

public class PendulumHinge : MonoBehaviour {

	Pendulum pen;

	Quaternion first;
	Quaternion second;

	//private Vector3 TargetEuler;
	//private Vector3 CurrentEuler;
	//private Vector3 BufferEuler;

	GameObject hinge;

	//currently targeting the second Euler angle?
	//bool target; 

	// Used for the size of the swing.
	float degreeSwing = 20f;
	//public float swingSpeedMult = 1f;
	//private float currSwingSpeed;
	//private float fastSwingSpeed = 1f;
	//private float slowSwingSpeed = 0.1f;

	//private float changeTime;
	public float changeRate = 4f;

	// Used to modify the time variable to integrate with the time slowing capabilities.
	// The current time%rate/rate apon slowing is saved, and then added to the time when the rate gets changed to rate*10
	// When time has resumed, the stageMod is transformed to changeMod%changeRate, and then is applied to Time.time
	//private float stageMod;

	float stage { get { return ((Time.time + timeMod)) % changeRate / changeRate; } }

	float slowAmount = 10f;
	float timeMod;
	//float slowedTime;

	// Used to retrieve a value between 0 and 1, to determine how close to Second the angle will be.
	float function { get { return -0.5f * Mathf.Cos (2 * Mathf.PI * stage) + 0.5f; } }

	// Use this for initialization
	void Start () {
		
		hinge = this.gameObject;

		pen = hinge.GetComponentInChildren<Pendulum> ();

		first = Quaternion.Euler (new Vector3 (degreeSwing / 2, 0, 0));
		second = Quaternion.Euler (new Vector3 (-degreeSwing / 2, 0, 0));

		//target = false;

		//TargetEuler = First;
	}
	
	// Update is called once per frame
	void Update () {

		if (pen.SlowedTime) {
			timeMod -=  Time.deltaTime - (Time.deltaTime / slowAmount);
		}

		MovePendulum ();
	}


	void MovePendulum() {
		hinge.transform.rotation = Quaternion.RotateTowards (first, second, degreeSwing * function);
	}

	/*
	void MovePendulum() {

		if (ItsTime()) {
			changeTime = Time.time + changeRate;

			target = !target;
			TargetEuler = !target ? EulerFirst : EulerSecond;
		}

		CheckSlowTime ();

		if (pen.SlowedTime) {
			changeTime += Time.deltaTime * (fastSwingSpeed - slowSwingSpeed);
		}

		BufferEuler = Vector3.Lerp (BufferEuler, TargetEuler, Time.deltaTime * currSwingSpeed * swingSpeedMult);
		CurrentEuler = Vector3.Lerp (CurrentEuler, BufferEuler, Time.deltaTime * currSwingSpeed * swingSpeedMult);

		//if (NearAngles ()) {
			//CurrentEuler = TargetEuler;
			//BufferEuler = CurrentEuler;
		//}

		hinge.transform.rotation = Quaternion.Euler (CurrentEuler);
	}

	bool ItsTime() {
		return Time.time > changeTime;
	}

	//bool AtAngle() {
	//	return (!target && (hinge.transform.rotation.eulerAngles == EulerFirst)) || (target && (hinge.transform.rotation.eulerAngles == EulerSecond));
	//}*/


	//PEN.SLOWEDTIME IS THE VARIABLE TO CHECK IF TIME IS SLOWED
	//void CheckSlowTime() {
	//	currSwingSpeed = pen.SlowedTime ? slowSwingSpeed : fastSwingSpeed;
	//}

	/*bool NearAngles() {
		Vector3 checkVec;
		checkVec = TargetEuler - CurrentEuler;

		return checkVec.magnitude < 0.01f;
	}*/
}
