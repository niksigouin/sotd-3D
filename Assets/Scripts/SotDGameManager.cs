using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SotDGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab != null)
            {
                int randomPoint = Random.Range(-20, 20);
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 10f, randomPoint), Quaternion.identity);
            }
            else
            {
                Debug.Log("PLAYER PREFAB IS NULL");
            }
            


        }
    }

    //public void OnLeaveButtonClicked()
    //{
    //    StartCoroutine(LeaveAndLoad());
    //}
    
    //IEnumerator LeaveAndLoad()
    //{
    //    Debug.Log("ENUMERATOR CALLED");
    //    PhotonNetwork.LeaveRoom();
    //    while (PhotonNetwork.InRoom)
    //        yield return null;
    //    SceneManager.LoadScene(0);
    //}

}
