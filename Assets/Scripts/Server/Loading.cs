using UnityEngine;
using System.Collections;

public enum LoadingScenes
{
    Menu,
    Game1
}

public class Loading : Photon.MonoBehaviour
{
    private static LoadingScenes NextScene { get; set; }

    private bool isOffline = false;

    void Start()
    {
        Debug.Log("Loading - Start + NextScene = " + NextScene.ToString() + " | Offline - " + PhotonNetwork.offlineMode);
        if (NextScene == LoadingScenes.Menu)
        {
            if (PhotonNetwork.offlineMode)
            {
                isOffline = true;
                OnJoinedLobby();                
            }
            else
                StartCoroutine(JointLobby());
        }
        if (NextScene == LoadingScenes.Game1 && isOffline == true)
        {
        
            OnJoinedRoom();
            Debug.Log("OnJoinedRoom()");
        }
    }

    private IEnumerator JointLobby()
    {
        while (PhotonNetwork.networkingPeer.State != ClientState.ConnectedToMaster)
            yield return new WaitForFixedUpdate();
        PhotonNetwork.networkingPeer.OpJoinLobby(TypedLobby.Default);
    }

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.LoadLevel(Config.SceneMenu);
    }
    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel(Config.SceneGame);
    }

    public static void Load(LoadingScenes _nextScene)
    {
        NextScene = _nextScene;
        Debug.Log(Config.SceneLoading + " - Load");
        PhotonNetwork.LoadLevel(Config.SceneLoading);
    }
}
