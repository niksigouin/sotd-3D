using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuController : MonoBehaviour
{
    [Header("Scenes")]
    public string MatchMakingScene;
    public GameObject Pause_UI_Panel;
    public GameObject Settings_UI_Panel;

    [Header("Toggles")]
    public bool isPaused;

    private int activeScene;

    // Update is called once per frame
    void Update()
    {
        CheckIfPaused();
    }


    #region Public Methods

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    public void OnLeaveButtonClicked()
    {
        Debug.Log("LEAVE LOBBY");
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
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isPaused = true;
                Cursor.lockState = CursorLockMode.None; // UNLOCK CURSOR
                Pause_UI_Panel.SetActive(true);
            }
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        //SetPlayerMouvState(true);
        Cursor.lockState = CursorLockMode.Locked;
        Pause_UI_Panel.SetActive(false);
    }

    
    #endregion
}