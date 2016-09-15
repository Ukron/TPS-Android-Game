using UnityEngine;
using System.Collections;

public class Menu : Photon.MonoBehaviour
{
    void Start()
    {
        Debug.Log("Menu");

        if (PhotonNetwork.offlineMode)
        {
            PhotonNetwork.CreateRoom("OfflineRoom");
        }
        else
        {
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions(), TypedLobby.Default);
        }

        Loading.Load(LoadingScenes.Game1);
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom Menu");
        PhotonNetwork.LoadLevel(Config.SceneGame);
    }
}
