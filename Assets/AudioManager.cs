using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;
    public float FadeDesired;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!source.isPlaying)
        {
            
            currentClip = audioClips[Random.Range(0, audioClips.Count)];
            source.clip = currentClip;
            source.Play();
            StartCoroutine(FadeAudioSource.StartFade(source, 5.0f, FadeDesired));
            
            
        }
    }
}
