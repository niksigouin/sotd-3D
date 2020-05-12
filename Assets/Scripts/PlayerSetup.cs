using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

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
            transform.GetComponent<MouvementController>().enabled = true;
            playerCamera.GetComponent<Camera>().enabled = true;

        }
        else
        {
            transform.GetComponent<MouvementController>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = false;
        }

        SetPlayerUI();

    }

    void SetPlayerUI()
    {
        if(playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
        
    }
}
