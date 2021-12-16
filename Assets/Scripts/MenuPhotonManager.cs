using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuPhotonManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player" + Random.Range(10, 99);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("On connect to master!");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 6});
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}