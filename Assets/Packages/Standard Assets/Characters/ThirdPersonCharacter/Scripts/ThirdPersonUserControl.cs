 // CUSTOM CHANGES IN THIRDPERSON USER CONTROL
// LOOK FOR ARROWS <---

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacterNEW))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
		public bool isLooking; // <------------------ CHANGE
		public bool isAiming; // <---------------- CHANGE
		public bool cameraRight = true; // <----------------- CHANGE
		public Cameras.FreeLookCam freeLookCamera; // <------------- CHANGE // CAMERA OBJECT
		private float camDist = 4f;
		private float blockDist = 1f;

		public bool movementActive; // <--------------- CHANGE

		//private GameObject currentPuzzle; // <---------------- CHANGE

		//Store the values of the camera so that manipulation can be easier, {Pivot point, Camera transform}
		private Vector3[] cameraDistancesRight; // <---------------- CHANGE
		private Vector3[] cameraDistancesLeft; // <---------------- CHANGE
		private Vector3[] cameraZeros; // <---------------- CHANGE
		private Vector3[] cameraAiming; // <---------------- CHANGE


        private ThirdPersonCharacterNEW m_Character; // A reference to the ThirdPersonCharacter on the object
        private Player m_Player;
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

		private float runSpeed = 1f;
		private float walkSpeed = 0.6f;
        
        private void Start()
        {

			cameraDistancesRight = new Vector3[2] {new Vector3(0.75f,1.25f,0f), new Vector3(0f,0f,-camDist)}; // <---------------- CHANGE
			cameraDistancesLeft = new Vector3[2] {new Vector3(-0.75f,1.25f,0f), new Vector3(0f,0f,-camDist)}; // <---------------- CHANGE
			cameraZeros = new Vector3[2] {Vector3.zero, Vector3.zero}; // <---------------- CHANGE
			cameraAiming = new Vector3[2] {cameraRight ? cameraDistancesRight [0] : cameraDistancesLeft [0], new Vector3(0f,0f,-blockDist)}; // <---------------- CHANGE

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
				freeLookCamera = GameObject.Find("FreeLookCameraRig").GetComponent<Cameras.FreeLookCam>(); // <---------------- CHANGE
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacterNEW>();

            m_Player = GetComponent<Player>();

			// Be sure the camera is set up correctly
			freeLookCamera.UpdateTarget (this.transform); // <---------------- CHANGE
			freeLookCamera.UpdateTransforms(cameraDistancesRight); // <---------------- CHANGE
        }

		public float GetCameraClipDistance() {
			return camDist;
		}

        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			freeLookCamera.aiming = isAiming;

			if (isAiming && !freeLookCamera.pivotCameraTransforms.Equals(cameraAiming)) {
				freeLookCamera.UpdateTransforms (cameraAiming);
				print (cameraAiming[0].ToString() + cameraAiming[1].ToString());
				freeLookCamera.ToggleCamClip(true);
			} else if (!isAiming && !isLooking && !freeLookCamera.pivotCameraTransforms.Equals(cameraRight ? cameraDistancesRight : cameraDistancesLeft)) {
				ResetCamera();
			}
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            /*if(!movementActive)
            {   // <----------------- CHANGE
                return;
            }*/
            // read inputs
            float h = movementActive ? CrossPlatformInputManager.GetAxis("Horizontal") : 0;
			float v = movementActive ? CrossPlatformInputManager.GetAxis("Vertical") : 0;
            bool crouch = false;// Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

                if(m_Player.IsMovingObject)
                    m_Move = v * m_Player.transform.forward;
                else
                    m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }

#if !MOBILE_INPUT
			// walk speed multiplier
			if (!Input.GetKey(KeyCode.LeftShift) || isLooking) { // <------------------ CHANGE
				m_Move *= walkSpeed;
			} else {
				m_Move *= runSpeed;
			}
#endif

			//if (Input.GetKeyDown (KeyCode.Q)) { // <---------------- CHANGE
			//	SwapCamera ();
			//}


            // pass all parameters to the character control script
			m_Character.Move(m_Move, crouch, m_Jump, isAiming);
            m_Jump = false;
        }

		private void SwapCamera() { // <---------------- CHANGE
			cameraRight = !cameraRight;
			ResetCamera ();
		}

		public void ResetCamera() { // <---------------- CHANGE
			if (cameraRight) {
				freeLookCamera.UpdateTransforms (cameraDistancesRight);
			} else {
				freeLookCamera.UpdateTransforms (cameraDistancesLeft);
			}

			cameraAiming[0] = cameraRight ? cameraDistancesRight [0] : cameraDistancesLeft [0];
		}

		/// <summary>
		/// Resets the camera, and also resets the target back to the character.
		/// </summary>
		/// <param name="resetTarget">This int doesn't change anything, it just identifies this version of the method.</param>
		public void ResetCamera(int resetTarget) {
			freeLookCamera.SetTarget (transform);
			ResetCamera ();
		}

		public void SetCameraToZeros() { // <------------------- CHANGE
			freeLookCamera.UpdateTransforms (cameraZeros);
		}

		//void OnTriggerEnter(Collider o) { // <------------------- CHANGE
		//	if (o.gameObject.tag.ToLower () == "puzzlemanager") {
		//		currentPuzzle = o.gameObject;
		//	}
		//}

		//void OnTriggerExit(Collider o) { // <------------------- CHANGE
		//	if (o.gameObject.tag.ToLower () == "puzzlemanager") {
		//		currentPuzzle = null;
		//	}
		//}
    }
}
