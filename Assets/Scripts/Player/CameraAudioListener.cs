using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudioListener : MonoBehaviour
{
    public Transform camera1;
    public Transform camera2;

    private void Update() { transform.position = (camera1.position + camera2.position) / 2; }
}
