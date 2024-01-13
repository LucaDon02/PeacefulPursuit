using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Manages audio in the game, ensuring only one instance exists. 
    /// Utilizes the Singleton pattern to enforce a single instance.
    /// Allows for the addition of audio files through the inspector and provides methods for playback control.
    /// </summary>
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
