using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    public void LeaveGame()
    {
        StartCoroutine(LeaveAndLoad());
    }

    IEnumerator LeaveAndLoad()
    {
        //PhotonNetwork.LeaveRoom();
        //while (PhotonNetwork.InRoom)
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(0);
    }
}
