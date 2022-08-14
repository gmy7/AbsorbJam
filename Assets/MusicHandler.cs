using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioSource src;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        src.clip = menuMusic;
        src.volume = SoundSettings.musicSound;
        src.Play();
    }
    public void UpdateSound()
    {
        src.volume = SoundSettings.musicSound;
    }

}
