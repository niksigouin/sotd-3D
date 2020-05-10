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

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine )
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
