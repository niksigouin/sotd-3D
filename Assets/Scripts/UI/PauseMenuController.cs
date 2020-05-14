using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace com.signik.sotd
{
    public class PauseMenuController : MonoBehaviourPunCallbacks
    {
        [Header("Scenes")]
        public GameObject Main_UI_Panel;
        public GameObject Settings_UI_Panel;

        [Header("Toggles")]
        public static bool paused = false;

        private bool disconnecting = false;

        #region Unity Methods

        private void Start()
        {
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion

        #region Public Methods

        public void OnResumeButtonClicked()
        {
            TogglePause();
        }

        public void OnLeaveButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.LeaveLobby();
            SceneManager.LoadScene(0);

            ////Debug.Log("LEAVE LOBBY");
            //DisconnectPlayer();
        }

        public void OnQuitButtonClicked()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }

        public void OnSettingsButtonClicked()
        {
            ActivatePanel(Settings_UI_Panel.name);
        }

        public void OnSettingsBackButtonClicked()
        {
            ActivatePanel(Main_UI_Panel.name);
        }

        public void ActivatePanel(string panelToBeActivated)
        {
            Main_UI_Panel.SetActive(panelToBeActivated.Equals(Main_UI_Panel.name));
            Settings_UI_Panel.SetActive(panelToBeActivated.Equals(Settings_UI_Panel.name));
        }

        public void TogglePause()
        {
            if (disconnecting) return;

            paused = !paused;

            transform.GetChild(0).gameObject.SetActive(paused);

            Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
        }
        #endregion

        #region Private Methods

        private void DisconnectPlayer()
        {
            Debug.Log("DISCONNECT PLAYER");
        }
        #endregion
    }
}