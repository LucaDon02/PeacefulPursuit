using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isForPlayer1;
    
    private Transform target;
    private Vector3 offset;

    void Start()
    {
        target = (isForPlayer1 ? GameObject.Find("Player1") : GameObject.Find("Player2")).transform;
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.6f);
    }
}
