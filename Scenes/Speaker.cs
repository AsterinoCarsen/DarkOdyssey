using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public GameObject soundPrefab;

    public void PlaySound(AudioClip soundEffect, float volume, bool randomizePitch)
    {
        GameObject i = Instantiate(soundPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

        i.GetComponent<AudioSource>().volume = volume;

        if (randomizePitch == true)
        {
            i.GetComponent<AudioSource>().pitch = Random.Range(1, 3);
        }

        i.GetComponent<AudioSource>().clip = soundEffect;
        i.GetComponent<AudioSource>().Play();

        Destroy(i, soundEffect.length);
    }
}
