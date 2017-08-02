using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Necromatic.Character
{
    [RequireComponent(typeof(CharacterMovement))]
    public class CharacterMovementInput : MonoBehaviour
    {
        private CharacterMovement m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam; // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward; // The current forward direction of the camera
        private Vector3 m_Move;


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<CharacterMovement>();
        }

        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }

            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;

            m_Character.Move(m_Move);
        }
    }
}
