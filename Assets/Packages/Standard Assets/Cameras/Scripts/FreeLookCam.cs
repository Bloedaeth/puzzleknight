// CUSTOM CHANGES IN FREE LOOK CAM
// LOOK FOR ARROWS <---

using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Cameras
{
    public class FreeLookCam : PivotBasedCameraRig
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Camera

		public Vector3[] pivotCameraTransforms; // <------------------- CHANGE
		public GameObject camPivot; // <--------------------- CHANGE
		public GameObject camObject; // <--------------------- CHANGE
		private ProtectCameraFromWallClipUnaltered freeLookCameraClipper;  // <--------------------- CHANGE

		public bool aiming; // <------------------- CHANGE
		public bool orbitActive; // <------------------- CHANGE
		public bool hideCursor = false; // <------------------ CHANGE

        [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
        [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
        [SerializeField] private float m_TurnSmoothing = 0.0f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
        [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
        [SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
        [SerializeField] private bool m_LockCursor;                           // Whether the cursor should be hidden and locked.
        [SerializeField] private bool m_VerticalAutoReturn = false;           // set wether or not the vertical axis should auto return

        private float m_LookAngle;                    // The rig's y axis rotation.
        private float m_TiltAngle;                    // The pivot's x axis rotation.
        private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
		private Vector3 m_PivotEulers;
		private Quaternion m_PivotTargetRot;
		private Quaternion m_TransformTargetRot;

        protected override void Awake()
        {
			orbitActive = true; // <------------------- CHANGE

            base.Awake();
            // Lock or unlock the cursor.
			m_LockCursor = hideCursor; // <------------------ CHANGE
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
			m_PivotEulers = m_Pivot.rotation.eulerAngles;

	        m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;

			freeLookCameraClipper = GetComponent<ProtectCameraFromWallClipUnaltered>();  // <--------------------- CHANGE
        }


        protected void Update()
        {
			m_LockCursor = hideCursor; // <------------------ CHANGE
			Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !m_LockCursor;

			UpdateCameraPositions (pivotCameraTransforms); //<----------------------- CHANGE

			if (!orbitActive) { // <------------------ CHANGE
				return;
			}

            HandleRotationMovement();
		}

		public void BreakRig(Transform newParent) {
			camObject.transform.parent = newParent;
		}

		public void BreakRig() {
			camObject.transform.parent = null;
		}

		public void FixRig() {
			camObject.transform.parent = camPivot.transform;
		}

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

		public void UpdateTransforms(Vector3[] newTransforms) { //<----------------------- CHANGE
			pivotCameraTransforms = newTransforms;
		}

		public void UpdateTarget(Transform newTarget) { //<----------------------- CHANGE
			m_Target = newTarget;
		}

		public void ToggleCamClip () {
			freeLookCameraClipper.disable = !freeLookCameraClipper.disable;
		}

		public void ToggleCamClip (bool disable) {
			freeLookCameraClipper.disable = disable;
		}

		public void UpdateCameraPositions(Vector3[] transforms) { //<---------------- CHANGE

            //FIXME
            //temp fix to the constant errors in the console haha
            // - Sean
            if(pivotCameraTransforms.Length < 1)
                return;
            //endfixme

			if (transforms [0] != pivotCameraTransforms [0] || transforms [1] != pivotCameraTransforms [1]) {
				pivotCameraTransforms = transforms;
			}

			if (camPivot.transform.localPosition != pivotCameraTransforms [0]) {
				camPivot.transform.localPosition = 
					Vector3.Lerp (camPivot.transform.localPosition, 
						pivotCameraTransforms [0], m_MoveSpeed * Time.deltaTime);
			}

			if (camObject.transform.localPosition != pivotCameraTransforms [1]) {
				camObject.transform.localPosition = 
					Vector3.Lerp (camObject.transform.localPosition, 
					pivotCameraTransforms [1], m_MoveSpeed * Time.deltaTime);
			}
			
			if (freeLookCameraClipper.disable && !aiming
				&& (pivotCameraTransforms [1] + -camObject.transform.localPosition).magnitude < 0.01f
				&& m_Target.gameObject.tag.ToLower() == "player") {
				ToggleCamClip ();
			}
		}

        protected override void FollowTarget(float deltaTime)
        {
            if (m_Target == null) return;
            // Move the rig towards target position.
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime*m_MoveSpeed);
        }

        private void HandleRotationMovement()
        {
			if (camObject.transform.localRotation.eulerAngles != Vector3.zero) {
				camObject.transform.localRotation = Quaternion.Euler (Vector3.zero);
			}


			if(Time.timeScale < float.Epsilon)
			return;

            // Read the user input
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            m_LookAngle += x*m_TurnSpeed;

            // Rotate the rig (the root object) around Y axis only:
            m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

            if (m_VerticalAutoReturn)
            {
                // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
                // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
                // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
                m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
            }
            else
            {
                // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
                m_TiltAngle -= y*m_TurnSpeed;
                // and make sure the new value is within the tilt range
                m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
            }

            // Tilt input around X is applied to the pivot (the child of this object)
			m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);

			if (m_TurnSmoothing > 0)
			{
				m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			}
			else
			{
				m_Pivot.localRotation = m_PivotTargetRot;
				transform.localRotation = m_TransformTargetRot;
			}
        }

		public void HandleRotationMovement(Quaternion rotation) {
			Vector3 eulers = rotation.eulerAngles;

			m_TransformTargetRot = Quaternion.Euler (0f, eulers.y, 0f);
			m_PivotTargetRot = Quaternion.Euler(eulers.x, m_PivotEulers.y , m_PivotEulers.z);

			if (m_TurnSmoothing > 0)
			{
				m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			}
			else
			{
				m_Pivot.localRotation = m_PivotTargetRot;
				transform.localRotation = m_TransformTargetRot;
			}
		}
    }
}
