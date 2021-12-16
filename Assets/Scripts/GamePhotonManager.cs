using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GamePhotonManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.LogFormat("Player {0} entered room, id = {1}", player.NickName, player.UserId);
    }
}