using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using PlayerInputController;

public class MenuController : MonoBehaviour
{
    [Header("Scenes")]
    public string MatchMakingScene;
    public GameObject Pause_UI_Panel;
    public GameObject Settings_UI_Panel;

    [Header("Toggles")]
    public bool isPaused;

    private int activeScene;

    private MouseLook mouseLook = new MouseLook();

    #region Unity Methods

    // Update is called once per frame
    private void Update()
    {
        CheckIfPaused();
    }

    #endregion

    #region Public Methods

    public void OnResumeButtonClicked()
    {
        ResumeGame();
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
        ActivatePanel(Pause_UI_Panel.name);
    }

    public void ActivatePanel(string panelToBeActivated)
    {
        Pause_UI_Panel.SetActive(panelToBeActivated.Equals(Pause_UI_Panel.name));
        Settings_UI_Panel.SetActive(panelToBeActivated.Equals(Settings_UI_Panel.name));
    }

    #endregion

    #region Private Methods

    private void CheckIfPaused()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // CHECKS IF IN GAME
        {
            Debug.Log("ESCAPED!");
            if (isPaused)
            {
                ResumeGame();
                mouseLook.SetCursorLock(true);
                //PlayerInputState(true);
            }
            else
            {
                isPaused = true;
                //PlayerInputState(false);
                mouseLook.SetCursorLock(false);
                Pause_UI_Panel.SetActive(true);
                
            }
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        //mouseLook.SetCursorLock(true);
        Pause_UI_Panel.SetActive(false);
    }

    private void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene(0);
    }

    private void PlayerInputState(bool state)
    {
        
    }

    #endregion
}