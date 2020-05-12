using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using PlayerInputController;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject playerCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    [Header("Debug")]
    public bool LocalControl;


    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine || LocalControl)
        {
            transform.GetComponent<PlayerController>().enabled = true;
            playerCamera.GetComponent<Camera>().enabled = true;

        }
        else
        {
            transform.GetComponent<PlayerController>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = false;
        }

        SetPlayerUI();

    }

    #region Private Methods

    void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }

    }

    #endregion

}