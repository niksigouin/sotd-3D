using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    //public Text connectionStatusText_OLD;
    public TextMeshProUGUI connectionStatusText;

    [Header("Login UI Panel")]
    public GameObject Login_UI_Panel;
    //public InputField playerNameInput_old; // Player name Input (Change to TMPro?)
    public TMP_InputField playerNameInput;
    

    // PANELS
    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    //public InputField roomNameInputField_old; // Room name Input (Change to TMPro?)
    //public InputField roomMaxPlayersInputField_old;
    public TMP_InputField roomNameInputField; // Room name Input (Change to TMPro?)
    public TMP_InputField roomMaxPlayersInputField;


    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI roomPlayersText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;
    public GameObject startGameButton;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameObject;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    [Header("Settings UI Panel")]
    public GameObject Settings_UI_Panel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(Login_UI_Panel.name);

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "" + PhotonNetwork.NetworkClientState;
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

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(RoomList_UI_Panel.name);

    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();

            ActivatePanel(GameOptions_UI_Panel.name);
        }
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        ActivatePanel(JoinRandomRoom_UI_Panel.name);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnStartGameButtonClicked()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
        
    }

    public void OnSettingsButtonClicked()
    {
        ActivatePanel(Settings_UI_Panel.name);
    }

    public void OnSettingsBackButtonClicked()
    {
        ActivatePanel(Login_UI_Panel.name);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
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

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }

        UpdateRoomInfo();

        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }
        
        // Instantiate player list gameObjects5
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameObject = Instantiate(playerListPrefab);
            playerListGameObject.transform.SetParent(playerListContent.transform);
            playerListGameObject.transform.localScale = Vector3.one;

            playerListGameObject.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = player.NickName;

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }

            if (player.IsMasterClient)
            {
                playerListGameObject.transform.Find("HostIndicator").gameObject.SetActive(true);
            }
            else
            {
                playerListGameObject.transform.Find("HostIndicator").gameObject.SetActive(false);
            }

            playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomInfo();

        GameObject playerListGameObject = Instantiate(playerListPrefab);
        playerListGameObject.transform.SetParent(playerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;

        playerListGameObject.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = newPlayer.NickName;

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }

        if (newPlayer.IsMasterClient)
        {
            playerListGameObject.transform.Find("HostIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerListGameObject.transform.Find("HostIndicator").gameObject.SetActive(false);
        }

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomInfo();

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptions_UI_Panel.name);

        foreach (GameObject playerListGameObject in playerListGameObjects.Values)
        {
            Destroy(playerListGameObject);
        }

        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            // If room is not open or is not visible or its been removed from the room list, remove it from the cached list
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                // Update cached room list
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList[room.Name] = room;
                }
                // Add new room to cached room list
                else
                {
                    cachedRoomList.Add(room.Name, room);
                }
            }
        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab);
            roomListEntryGameObject.transform.SetParent(roomListParentGameObject.transform);
            roomListEntryGameObject.transform.localScale = Vector3.one;

            roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = room.Name;
            roomListEntryGameObject.transform.Find("RoomPlayersText").GetComponent<TextMeshProUGUI>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomListEntryGameObject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

            roomListGameObjects.Add(room.Name, roomListEntryGameObject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedRoomList.Clear();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        string roomName = "Room " + Random.Range(100, 1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
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
        Settings_UI_Panel.SetActive(panelToBeActivated.Equals(Settings_UI_Panel.name));
    }


    #endregion

    #region Private Methods
    void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
    }

    void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomListGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }

        roomListGameObjects.Clear();
    }

    void UpdateRoomInfo()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomPlayersText.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    #endregion
}
