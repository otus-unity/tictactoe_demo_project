using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuPhotonManager : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player" + Random.Range(10, 99);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("On connected to master!");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 6});
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}