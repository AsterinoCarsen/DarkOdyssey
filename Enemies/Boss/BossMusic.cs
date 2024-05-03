using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    private AudioSource speaker;
    public AudioClip bossMusic;

    void Start()
    {
        speaker = GetComponent<AudioSource>();
    }

    public void ChangeMusic()
    {
        StartCoroutine(changeMusicBridge());
    }

    private IEnumerator changeMusicBridge()
    {
        speaker.Stop();

        yield return new WaitForSeconds(4);

        speaker.clip = bossMusic;
        speaker.Play();
    }

}
