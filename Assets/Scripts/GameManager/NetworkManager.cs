using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    public GameObject standbyCamera;
    [SerializeField]
    private GameObject crossHair;
    private SpawnSpot[] spawnSpots;

    List<string> chatMessages;
    int maxChatMessages = 5;

    public float respawnTimer = 1;

    void Start()
    {
        spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
        chatMessages = new List<string>();
    }

    void Update()
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0)
                SpawnMyPlayer();
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connected)
        {
            GUILayout.BeginVertical();
            foreach (string message in chatMessages)
            {
                GUILayout.Label(message);
            }
            GUILayout.EndVertical();
        }
    }


    void SpawnMyPlayer()
    {
        AddChatMessage("Spawning Player: " + PhotonNetwork.player.name);

        if (spawnSpots == null)
        {
            Debug.LogError("spawnSpots == null");
            return;
        }

        SpawnSpot mySpawnSpot = spawnSpots[Random.Range(0, spawnSpots.Length)];

        GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate
            ("Player", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
        standbyCamera.SetActive(false);

        //////////////Test//////////////
        myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);


        //myPlayerGO.GetComponent<PlayerMovement>().enabled = true;
        //myPlayerGO.GetComponent<PlayerShooting>().enabled = true;

        //myPlayerGO.GetComponent<Animator>().enabled = true;
        //////////////Test//////////////

        //myPlayerGO.GetComponent<PlayerController>().enabled = true;
        //myPlayerGO.GetComponent<NetworkCharacter>().enabled = true;
        //myPlayerGO.GetComponent<PlayerMotor>().enabled = true;
    }
    public void AddChatMessage(string m)
    {
        GetComponent<PhotonView>().RPC("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
    }
    [PunRPC]
    void AddChatMessage_RPC(string m)
    {
        while (chatMessages.Count >= maxChatMessages)
        {
            chatMessages.RemoveAt(0);
        }
        chatMessages.Add(m);
    }

    //void Connect()
    //{
    //    PhotonNetwork.ConnectUsingSettings("PhotonTPS v002");
    //}
    //void OnJoinedLobby()
    //{
    //    PhotonNetwork.JoinRandomRoom();
    //}
    //void OnPhotonRandomJoinFailed()
    //{
    //    Debug.Log("OnPhotonRandomJoinFailed");
    //    PhotonNetwork.CreateRoom(null);
    //}
    //void OnJoinedRoom()
    //{
    //    Debug.LogError("OnJoinedRoom");
    //    SpawnMyPlayer();
    //    background.SetActive(false);
    //    crossHair.SetActive(true);
    //    joysticks.SetActive(true);
    //}

    //public void OnButtonStartMenu(bool _isSingle)
    //{
    //    PhotonNetwork.player.name = startMenu.GetComponentInChildren<InputField>().text;
    //    startMenu.SetActive(false);

    //    if (_isSingle)
    //    {
    //        PhotonNetwork.offlineMode = true;
    //        OnJoinedLobby();
    //    }
    //    else
    //    {
    //        Connect();
    //    }
    //}


    //// My Code Start// изменение размера джойстиков относительно размера экрана
    //float joystickSize = Screen.width >= Screen.height ? Screen.height / 3.5f : Screen.width / 3.5f;

    //RectTransform joystickBaseRT = transform.GetChild(0).GetComponent<RectTransform>();
    //RectTransform stickRT = transform.GetChild(1).GetComponent<RectTransform>();

    //joystickBaseRT.sizeDelta = new Vector2(joystickSize, joystickSize);
    //stickRT.sizeDelta = new Vector2(joystickSize / 1.5f, joystickSize / 1.5f);
    //// My Code End
}
