using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerInputController
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        #region Public Settings
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // FOWARD WALKING SPEED
            public float BackwardSpeed = 4.0f;  // BACKWARDS WALKING SPEED
            public float StrafeSpeed = 4.0f;    // SIDEWAYS WALKING SPEED
            public float RunMultiplier = 2.0f;   // SPRINT SPEED MUTLTIPLIER
            public KeyCode RunKey = KeyCode.LeftShift; // SETS SPRINT KEY ( MAYBE CHANGE LATER TU USER UNITY INPUTS)
            public float JumpForce = 30f; // JUMP FORCE
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

            private bool m_Running;

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
                if (input == Vector2.zero) return;
                if (input.x > 0 || input.x < 0)
                {
                    // SIDEWAYS
                    CurrentTargetSpeed = StrafeSpeed;
                }
                if (input.y < 0)
                {
                    // BACKWARDS
                    CurrentTargetSpeed = BackwardSpeed;
                }
                if (input.y > 0)
                {
                    // FORWARD
                    CurrentTargetSpeed = ForwardSpeed;
                }

                if (Input.GetKey(RunKey))
                {
                    // IF SPRINTING
                    CurrentTargetSpeed *= RunMultiplier;
                    m_Running = true;
                }
                else
                {
                    m_Running = false;
                }
            }

            public bool Running
            {
                get { return m_Running; }
            }
        }

        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // DISTANCE FOR CHECKING IF PLAYER IS ON THE GROUND
            public float stickToGroundHelperDistance = 0.5f; // STOPS THE PLAYER
            public float slowDownRate = 20f; // RATE OF STOP WHEN NO INPUT
            public bool airControl; // TOGGLE CONTROLLING MID AIR
            public float shellOffset; //SET IT TO 0.1 OR MORE IF STUCK IN WALLS 
        }
        #endregion

        #region Variables

        public Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();

        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;

        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get { return movementSettings.Running; }
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            // SETS COMPONENTS & INITIALIZES MOUSE LOOK
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init(transform, cam.transform);
        }


        private void Update()
        {
            RotateView();

            // GETS JUMP INPUT ( CHANGE LAST OR PARAM IF YOU WANT DOUBLE JUMP)
            if (Input.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }
        }

        private void FixedUpdate()
        {
            GroundCheck();
            Vector2 input = GetInput();

            // MOVES THE PLAYER IN DIRECTION IF THE INPUTS ARE GREATE THAN THE SMALLES POSITIVE SINGLE VALUE AND EITHER AIRCONTROL OR ISGROUNDED IF TRUE
            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x *= movementSettings.CurrentTargetSpeed;
                desiredMove.z *= movementSettings.CurrentTargetSpeed;
                desiredMove.y *= movementSettings.CurrentTargetSpeed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                // IF NO MOVEMENT MAKE THE RIGID BODY SLEEP
                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    StickToGroundHelper();
                }
            }
            m_Jump = false;
        }

        #endregion

        #region Private Methods

        private float SlopeMultiplier()
        {
            // CALCULATES THE ANGLE OF SLOPE TO SEE IF PLAYER CAN GO UP
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }

        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height / 2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }

        private Vector2 GetInput()
        {
            // GETS USER MOVEMENT INPUT AS A VECT 2
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
            movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }

        private void RotateView()
        {
            // CAPTURES ROTATION BEFORE CHANGE
            float oldYRotation = transform.eulerAngles.y;

            // SENDS THE DATA TO MOUSELOOK
            mouseLook.LookRotation(transform, cam.transform);

            // CHECKS IF PLAYER IS GROUNDED OR IF AIR CONTROL IS ENABLED TO ROTATE THE PLAYER
            if (m_IsGrounded || advancedSettings.airControl)
            {
                // ROTATE THE RIGIDBODY
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
            }
        }

        private void GroundCheck()
        {
            // CAST A SPHERE ON THE BOTTON OF PLAYER TO CHECK IF THE PLAYER IS GROUNDED
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
            {
                m_Jumping = false;
            }
        }

        #endregion
    }
}

