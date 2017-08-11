using UnityEngine;

namespace Necromatic.Character
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] private float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] private float m_MoveSpeedMultiplier = 1f;
        [SerializeField] private float m_AnimSpeedMultiplier = 1f;
        [SerializeField] private Animator m_Animator;
        private Rigidbody m_Rigidbody;
        private const float k_Half = 0.5f;
        private float m_TurnAmount;
        private float m_ForwardAmount;
        private Vector3 m_GroundNormal;
        private float m_CapsuleHeight;
        private Vector3 m_CapsuleCenter;
        private CapsuleCollider m_Capsule;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;
        }

        public void Move(Vector3 move)
        {

            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            var rawMove = move;
            move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);

            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;
            if (rawMove != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(rawMove.x, rawMove.z) * Mathf.Rad2Deg, 0);
            }
            //Debug.Log(move);

            //ApplyExtraTurnRotation();
            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }

        private void UpdateAnimator(Vector3 move)
        {
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);

            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

            if (move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }
    }
}
