using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


namespace Com.Signik.Player
{
    public class PauseMenuController : MonoBehaviour
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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion

        #region Public Methods

        public void OnResumeButtonClicked()
        {
            TogglePause();
            //SetCursorLock(false);
        }

        public void OnLeaveButtonClicked()
        {
            Debug.Log("LEAVE LOBBY");
            DisconnectPlayer();
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

            StartCoroutine(DisconnectAndLoad());
        }

        IEnumerator DisconnectAndLoad()
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.IsConnected)
                yield return null;

            SceneManager.LoadScene(0);
        }

        private void SetCursorLock(bool state)
        {
            switch (state)
            {
                case true:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = false;
                    break;
                case false:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                default:
                    break;
            }
            
        }

        #endregion
    }
}