using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
