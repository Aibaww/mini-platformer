using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }
}
