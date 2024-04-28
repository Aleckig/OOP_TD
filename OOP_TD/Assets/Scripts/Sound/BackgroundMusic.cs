using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] songs; // List of background songs
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomSong();
    }

    void PlayRandomSong()
    {
        if (songs.Length > 0)
        {
            int randomIndex = Random.Range(0, songs.Length);
            audioSource.clip = songs[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No songs assigned to the BackgroundMusic script!");
        }
    }

    void Update()
    {
        // Check if the current song is finished playing
        if (!audioSource.isPlaying)
        {
            // Play a new random song
            PlayRandomSong();
        }
    }
}
