using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundSequentially : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip firstClip;
    public AudioClip secondClip;

    void Start()
    {
        StartCoroutine(PlaySoundsInSequence());
    }

    IEnumerator PlaySoundsInSequence()
    {
        // Play the first clip
        audioSource.clip = firstClip;
        audioSource.Play();
        
        // Wait until the first clip finishes
        yield return new WaitForSeconds(firstClip.length);
        
        // Play the second clip
        audioSource.clip = secondClip;
        audioSource.Play();

    }
}
