using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public float hitPoints = 100f;
    float currentHitPoints;

    // Use this for initialization
    void Start()
    {
        currentHitPoints = hitPoints;
    }

    [PunRPC]
    public void TakeDamage(float amt)
    {
        currentHitPoints -= amt;
        if (currentHitPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GetComponent<PhotonView>().instantiationId == 0)
        {
            Debug.Log("Die ID == 0 |  " + GetComponent<PhotonView>().instantiationId.ToString());
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Die Else");
            if (GetComponent<PhotonView>().isMine)
            {
                if (gameObject.tag == "Player")
                {
                    NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();

                    nm.standbyCamera.SetActive(true);
                    nm.respawnTimer = 3f;
                }
                //else if(gameObject.tag == "Bot")
                //{
                //    Debug.LogError("WARNING: No bot respawn code exists!");
                //}

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
