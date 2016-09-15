using UnityEngine;
using System.Collections;

public class NetworkCharacterBackup : Photon.MonoBehaviour
{
    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;

    Animator anim;

    bool gotFirstUpdate = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (photonView.isMine)
        {

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetFloat("Speed"));
            stream.SendNext(anim.GetBool("Resting"));
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            anim.SetFloat("Speed", (float)stream.ReceiveNext());
            anim.SetBool("Resting", (bool)stream.ReceiveNext());

            if (gotFirstUpdate == false)
            {
                transform.position = realPosition;
                transform.rotation = realRotation;
                gotFirstUpdate = true;
            }
        }
    }
}