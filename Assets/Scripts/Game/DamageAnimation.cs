using UnityEngine;
using UnityEngine.UI;

namespace Game
{ 
    ///<summary>
    /// Manages damage animation in the game, controlling the visual effect of damage received.
    /// Allows for customization of animation duration and smoothly adjusts the size of the associated Image component over time.
    ///</summary>

    public class DamageAnimation : MonoBehaviour
    {
        public bool start;
        public float animationDuration = 0.125f;

        private Image image;
        private float timer;
        private bool increasing = true;
        private const float BaseValue = 1000f;

        private void Start() { image = GetComponent<Image>(); }

        private void Update()
        {
            if (!start) return;
            timer += Time.unscaledDeltaTime;
        
            if (timer > animationDuration)
            {
                timer = 0f;
                increasing = !increasing;
                start = !increasing;
            }

            float t = Mathf.Clamp01(timer / animationDuration);
        
            float exponent = increasing ? 1 - t : t;
            float newPixelsPerUnit = Mathf.Pow(BaseValue, exponent);

            image.pixelsPerUnitMultiplier = newPixelsPerUnit;
        }
    }
}
