using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        Click,
        Door,
        BeraSFX
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f;
        GameObject.DontDestroyOnLoad(soundGameObject);
        audioSource.PlayOneShot(GetAudioClip(sound));
        GameObject.Destroy(soundGameObject, 5);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundClip in GameAssets.instance.soundAudioClipArray)
        {
            if (soundClip.sound == sound)
            {
                return soundClip.audioClip;
            }
        }
        return null;
    }
}