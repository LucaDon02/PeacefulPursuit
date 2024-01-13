/// <summary>
/// This class manages to audio in the game. There will always only be 1 instance of this class.
/// It uses Singleton to ensure this, the class allows for audio files to be added with the inspector
/// and played/pauzed with the method api.
/// <summary>

using System.Linq;
using UnityEngine;

namespace Game
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager Instance { get; set; }
        public Sound[] sounds;

        private void Awake()
        {
            InitializeSingleton();
            InitializeSounds();
            PlaySound("MainTheme");
        }
        private void InitializeSingleton()
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

        public void PlaySound(string soundName)
        {
            var soundToPlay = GetSoundByName(soundName);
            soundToPlay?.source.Play();
        }

        public void PauseSound(string soundName)
        {
            var soundToPause = GetSoundByName(soundName);
            soundToPause?.source.Pause();
        }
    

        private Sound GetSoundByName(string soundName)
        {
            return sounds.FirstOrDefault(s => s.Name == soundName);
        }
    }
}
