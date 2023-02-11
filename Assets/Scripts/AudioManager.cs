using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // check if the sound file name matches what its called in the reference
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found. Ensure the sound files' name matches up correctly in the reference.");
            return;
        }
        
        s.source.Play();
    }
}
