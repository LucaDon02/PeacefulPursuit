using UnityEngine;

namespace Game
{
    /// <summary>
    /// Sound model for the inspector GUI in Unity.
    /// This allows us to add more sounds with the GUI, which get mapped in the AudioManager class.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        [SerializeField] private string name;
        [SerializeField] private AudioClip clip;
        [Range(0f, 1f)] [SerializeField] private float volume;
        [Range(0.1f, 3f)] [SerializeField] private float pitch;
        [SerializeField] private bool loop;
        [HideInInspector] public AudioSource source;

        public string Name => name;
        public AudioClip Clip => clip;
        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;
    }
}
