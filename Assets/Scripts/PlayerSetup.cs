using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject playerCamera;
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



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
