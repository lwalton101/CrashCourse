using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sfxSounds;
    public Sound[] musicSounds;

    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup musicGroup;


    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {

        if(instance == null)
		{
            instance = this;
		}
		else
		{
            Destroy(gameObject);
            return;
		}

        DontDestroyOnLoad(gameObject);

        SetAudioSource();

        Play("Background1");
    }

	// Update is called once per frame
	public void Play(string name)
    {
       Sound s = Array.Find(sfxSounds, sound => sound.name == name);
       if(s == null)
        {
            s = Array.Find(musicSounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogError("Sound: " + name + " has not been found.");
                return;
            }
        }
       s.source.Play();
    }

    public void SetAudioSource()
	{
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        foreach(AudioSource audioSource in sources)
		{
            Destroy(audioSource);
		}
        foreach (Sound s in sfxSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = sfxGroup;
        }

        foreach (Sound s in musicSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = musicGroup;

        }

    }
}
