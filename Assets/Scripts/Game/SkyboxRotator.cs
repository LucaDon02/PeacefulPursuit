using UnityEngine;

namespace Game
{
    /// <summary>
    /// Rotates the skybox in the game environment, providing a dynamic visual effect.
    /// Adjusts the rotation speed based on the specified rotation per second.
    /// </summary>
    public class SkyboxRotator : MonoBehaviour
    {
        public float RotationPerSecond = 2;
        protected void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);
        }
    }
}