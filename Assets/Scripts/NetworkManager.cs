using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    public Text connectionStatusText;

    [Header("Login UI Panel")]
    public GameObject Login_UI_Panel;
    public InputField playerNameInput; // Player name Input (Change to TMPro?)
    

    // PANELS
    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    public InputField roomNameInputField; // Room name Input (Change to TMPro?)
    public InputField roomMaxPlayersInputField;

    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(Login_UI_Panel.name);
    }

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
    }

    #endregion

    #region UI Callbacks

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // WARN USER THAT THE NAME DOES NOT MEET STANDARDS
            Debug.Log("Playername is invalid");
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(100, 1000);


        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte) int.Parse(roomMaxPlayersInputField.text);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoom_UI_Panel.name);
    }

    #endregion

    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
        JoinRandomRoom_UI_Panel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Panel.name));


    }


    #endregion
}
