using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstance : MonoBehaviour
{
    public static SingleInstance instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }
    public void PlayMusic()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
