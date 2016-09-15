using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Login : Photon.MonoBehaviour
{
    public InputField inputFieldName;
    public Button button;

    void Start()
    {
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Name");
        inputFieldName.text = PhotonNetwork.player.name;

        PhotonNetwork.autoJoinLobby = false;
    }
    void Connecting()
    {
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings("PhotonTPS v006");
    }
    void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Loading.Load(LoadingScenes.Menu);
    }

    public void OnButtonStart(bool isSingle)
    {
        PhotonNetwork.player.name = inputFieldName.text;
        PlayerPrefs.SetString("Username", PhotonNetwork.player.name);

        button.interactable = false;

        if (isSingle)
            PhotonNetwork.offlineMode = true;
        else
            Connecting();
    }
}
