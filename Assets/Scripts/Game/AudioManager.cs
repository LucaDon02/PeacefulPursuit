using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public Sound[] sounds;

    void Awake()
    {
        InitalizeSingleton();
        InitializeSounds();
        PlaySound("MainTheme");
    }
    private void InitalizeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSounds()
    {
        foreach (var sound in sounds)
        {
            InitializeSound(sound);
        }
    }

    private void InitializeSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.Clip;
        sound.source.volume = sound.Volume;
        sound.source.loop = sound.Loop;
        sound.source.pitch = sound.Pitch;
    }

    public void PlaySound(string name)
    {
        var soundToPlay = GetSoundByName(name);
        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
    }

    public void PauseSound(string name)
    {
        var soundToPause = GetSoundByName(name);
        if (soundToPause != null)
        {
            soundToPause.source.Pause();
        }
    }
    

    private Sound GetSoundByName(string name)
    {
        return sounds.FirstOrDefault(s => s.Name == name);
    }
}
