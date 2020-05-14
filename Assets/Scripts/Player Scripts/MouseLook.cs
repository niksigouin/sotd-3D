using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.signik.sotd
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        public bool playerIsPaused = false;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        #region Public Methods

        // INITIALIZE THE STARTING ROTATIONS FOR PLAYER ELEMENTS
        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        // CALCULATE ROTATION OF PLAYER
        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            // IF CAMERA ROTATION CLAMPING IS ON, CLAMP THE CAMERA X ROTATION
            //if (clampVerticalRotation)
            //    xRot = ClampVerticalRotation(xRot);

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            // OLD CAMERA X ROTATION CLAMPING
            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            // IF SMOOTH CAMERA MOTION IS ON LERP THE POSITION
            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        //TOGGLE CURSOR LOCK STATE
        //public void SetCursorLock(bool value)
        //{
        //    lockCursor = value;
        //    if (!lockCursor)
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
        //}

        // UPDATE THE CURSOR LOCKSTATE
        public void UpdateCursorLock()
        {
            //if (m_cursorIsLocked)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //    Cursor.visible = false;

            //    if (Input.GetKeyDown(KeyCode.Escape))
            //    {
            //        m_cursorIsLocked = false;
            //    }
            //}
            //else
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //    Cursor.visible = true;

            //    if (Input.GetKeyDown(KeyCode.Escape))
            //    {
            //        m_cursorIsLocked = true;
            //    }
            //}
        }

        #endregion

        #region Private Methods

        //private void InternalLockUpdate()
        //{
        //    //if (Input.GetKeyUp(KeyCode.Escape))
        //    //{
        //    //    if (playerIsPaused)
        //    //    {
        //    //        //Menu_Controller.GetComponent<MenuController>().isPaused = false;
        //    //        //playerIsPaused = false;
        //    //        m_cursorIsLocked = false;
        //    //    } else if(!playerIsPaused)
        //    //    {
        //    //        //Menu_Controller.GetComponent<MenuController>().isPaused = true;
        //    //        //playerIsPaused = true;
        //    //        m_cursorIsLocked = true;
        //    //    }
        //    //}

        //    if (m_cursorIsLocked)
        //    {
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //    }
        //    else if (!m_cursorIsLocked)
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
        //}

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        float ClampVerticalRotation(float angle)
        {
            angle = Mathf.Clamp(angle, MinimumX, MaximumX);
            return angle;
        }

        #endregion
    }
}