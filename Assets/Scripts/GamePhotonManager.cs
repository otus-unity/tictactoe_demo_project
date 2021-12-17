using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GamePhotonManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room, id = {1}", newPlayer.NickName, newPlayer.UserId);
    }
}