using GameLogging;
using UnityEngine;

public class PerspectiveButton : MonoBehaviour {

	public bool playerStanding;

	private bool isActive;
	private bool deactivateCalled;
	private float deactivateTime;
	private float deactivateDelay = 2f;

	public GameObject cameraPoint;

	private PerspectivePuzzle pp;

	UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl tpuc;

	// Use this for initialization
	private void Start() {
		isActive = true;
		deactivateCalled = false;

		pp = GetComponentInParent<PerspectivePuzzle> ();
	}
	
	// Update is called once per frame
	private void Update() {
		if (isActive) {
			if (deactivateCalled) {
				if (Time.time > deactivateTime) {
					isActive = false;
				}
			}
		}
	}

    /// <summary>Deactivates the button, preventing further use.</summary>
	public void Deactivate()
    {
        BuildDebug.Log("Deactivating perspective button");
        deactivateTime = Time.time + deactivateDelay;
		deactivateCalled = true;
	}

    /// <summary>Gets the camera point game object.</summary>
	public GameObject GetCameraPoint() {
		return cameraPoint;
	}

	private void OnTriggerEnter(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player" && isActive)
        {
            BuildDebug.Log("Perspective button stepped on");
            playerStanding = true;
			tpuc = o.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ();

			if (pp.IndexesAllCorrect () && !pp.puzzleIsSolved) {
				pp.cc.BeginChase (1f);
			}
		}


	}

    private void OnTriggerExit(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player")
        {
            BuildDebug.Log("Perspective button stepped off");
            playerStanding = false;

			tpuc.isLooking = false;
			tpuc.ResetCamera ();
			tpuc.freeLookCamera.UpdateTarget (o.gameObject.transform);

			if (pp.cc.active) {
				pp.cc.EndChase ();
			}
		}


	}

    private void OnTriggerStay(Collider o) {
		if (o.gameObject.transform.tag.ToLower() == "player" && isActive && !pp.cc.active) {
			tpuc.isLooking = true;
			tpuc.SetCameraToZeros();
			tpuc.freeLookCamera.UpdateTarget (cameraPoint.transform);
			tpuc.freeLookCamera.ToggleCamClip(true);
		}

		pp.CheckCamPosition ();
	}
		
}
