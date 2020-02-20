using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonLobby : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonLobby INSTANCE;

    public GameObject battleBtn;
    public GameObject cancelBtn;
    public GameObject connectionStatusBtn;

    private void Awake()
    {
        if(PhotonLobby.INSTANCE == null)
        {
            PhotonLobby.INSTANCE = this;
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public void OnBattleBtnClicked()
    {
        battleBtn.SetActive(false);
        cancelBtn.SetActive(true);
        // Join Random Room
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelBtnClicked()
    {
        battleBtn.SetActive(true);
        cancelBtn.SetActive(false);
        // Leave the room
        PhotonNetwork.LeaveRoom();
    }

    private void CreateRoom()
    {
        int random = Random.Range(1000, 9999);
        RoomOptions roomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, CleanupCacheOnLeave = true, MaxPlayers = 5 };

        // Create a random room with specified value
        PhotonNetwork.CreateRoom("Room" + random, roomOptions);
    }

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("Player has connected to photon master server");
        connectionStatusBtn.GetComponent<Image>().color = Color.green;
        battleBtn.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        connectionStatusBtn.GetComponent<Image>().color = Color.grey;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Trying to join a random room but failed to connect.");
        // Try to create a new room
        CreateRoom();
        //PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created Room -> Room Name :: " + PhotonNetwork.CurrentRoom.Name + "\nMax Player :: " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined the room with name = " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the lobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        // Try to create the room again
        Debug.Log("Tried to create a new room but failed, there must be a room available with the same name");
        CreateRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left the room");
    }

    #endregion
}
