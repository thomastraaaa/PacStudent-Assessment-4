using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip firstClip;
    public AudioClip secondClip;

    void Start()
    {
        StartCoroutine(PlaySoundIntro());
    }

    IEnumerator PlaySoundIntro()
    {
        audioSource.clip = firstClip;
        audioSource.Play();
        yield return new WaitForSeconds(firstClip.length);
        audioSource.clip = secondClip;
        audioSource.Play();

    }
}
