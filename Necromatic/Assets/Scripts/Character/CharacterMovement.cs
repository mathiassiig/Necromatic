using Necromatic.Char.Combat;
using UnityEngine;

namespace Necromatic.Char
{
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField]
        private float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField]
        private float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        private float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private Rigidbody m_Rigidbody;
        [SerializeField]
        private float m_TurnSpeed = 10f;
        [SerializeField]
        private CharacterCombat _combat;
        [SerializeField]
        private Transform _transformToRotate;
        private float m_ForwardAmount;

        public Animator M_Animator => m_Animator;
        


        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            m_Animator.SetFloat("WalkSpeed", m_AnimSpeedMultiplier);
        }

        public void Move(Vector3 move)
        {
            var rawMove = move;
            move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.zero);
            m_ForwardAmount = move.magnitude;
            UpdateAnimator(move, rawMove);
        }


        private void TurnTowards(Transform t)
        {
            var currentRotation = _transformToRotate.rotation;
            _transformToRotate.LookAt(t);
            var newRotation = _transformToRotate.rotation;
            _transformToRotate.rotation = Quaternion.Lerp(currentRotation, newRotation, m_TurnSpeed * Time.deltaTime);
        }

        private void UpdateAnimator(Vector3 move, Vector3 rawMove)
        {
            if (_combat && _combat.Attacking)
            {
                m_Rigidbody.velocity = Vector3.zero;
                m_Animator.SetFloat("Forward", 0);
                if (_combat.CurrentTarget != null)
                {
                    TurnTowards(_combat.CurrentTarget.gameObject.transform);
                }
                return;
            }
            if (rawMove != Vector3.zero)
            {
                _transformToRotate.rotation = Quaternion.Lerp(_transformToRotate.rotation, Quaternion.Euler(0, Mathf.Atan2(rawMove.x, rawMove.z) * Mathf.Rad2Deg, 0),
                    m_TurnSpeed * Time.deltaTime);
            }
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);

            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            var velocity = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * move;
            m_Rigidbody.velocity = m_MoveSpeedMultiplier * velocity / Time.deltaTime;
        }

    }
}
