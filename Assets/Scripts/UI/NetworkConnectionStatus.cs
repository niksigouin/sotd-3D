using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkConnectionStatus : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    public TextMeshProUGUI connectionStatusText;

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "" + PhotonNetwork.NetworkClientState;
    }
}
