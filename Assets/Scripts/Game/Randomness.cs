using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class Randomness : MonoBehaviour
    {
        private Volume volume;
        private ColorAdjustments colorAdjustments;
        public float[] randomHueShitf;

        private void Start()
        {
            volume = GetComponent<Volume>();
            volume.profile.TryGet(out colorAdjustments);
            colorAdjustments.hueShift.value = randomHueShitf[Random.Range(0, randomHueShitf.Length)];
        }
    }
}
