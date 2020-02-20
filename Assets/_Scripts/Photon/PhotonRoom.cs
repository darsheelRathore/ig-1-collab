using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public static PhotonRoom INSTANCE;
    private int roomBuildIndex = 1;

    private void Awake()
    {
        if(PhotonRoom.INSTANCE == null)
        {
            PhotonRoom.INSTANCE = this;
        }
        else
        {
            if(PhotonRoom.INSTANCE != this)
            {
                Destroy(PhotonRoom.INSTANCE.gameObject);
                PhotonRoom.INSTANCE = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        // Load game scene
        PhotonNetwork.LoadLevel(roomBuildIndex);
    }

    private void OnSceneFinishedLoading(Scene sceneName, LoadSceneMode mode)
    {
        int currentBuildIndex = sceneName.buildIndex;
        if(currentBuildIndex == roomBuildIndex)
        {
            // Instantiate player
            PhotonNetwork.Instantiate(Path.Combine("PlayerPrefab", "Player"), 
                Vector3.zero, Quaternion.identity);
        }
    }
}
