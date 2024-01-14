using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        public bool isForPlayer1;
    
        private Transform target;
        private Vector3 offset;

        private void Start()
        {
            target = (isForPlayer1 ? GameObject.Find("Player1") : GameObject.Find("Player2")).transform;
            offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.6f);
        }
    }
}
