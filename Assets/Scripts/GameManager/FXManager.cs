using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour
{
    public AudioClip BulletSound;
    // public AudioClip someSound; - sound for Hit Player

    [PunRPC]
    void BulletFX(Vector3 startPos, Vector3 endPos)
    {
        AudioSource.PlayClipAtPoint(BulletSound, startPos);
    }
}
