using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    // Start is called before the first frame update
    private void Awake()
    {
        foreach (Sound sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string soundName)
    {
        Sound sound = Array.Find(Sounds, sound => sound.name == soundName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found. Ensure the sound files' name matches up correctly in the reference.");
            return;
        }
        sound.source.Play();
    }
}
