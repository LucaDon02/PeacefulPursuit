using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float threshold = 100.0f;

    void FixedUpdate()
    {
        var cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (!(cameraPosition.magnitude + 10 >= threshold)) return;
        
        cameraPosition.z += 10;
        foreach (var g in SceneManager.GetActiveScene().GetRootGameObjects())
            if (g.layer == 9) g.transform.position -= cameraPosition;

        foreach (var tileManager in FindObjectsOfType<TileManager>()) tileManager.zSpawn = 60;

    }
}