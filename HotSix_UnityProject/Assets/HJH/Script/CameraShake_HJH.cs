using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake_HJH : MonoBehaviour
{
    Camera cam;
    Vector3 cameraPos;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    float shakeRange = 0.05f;
    [SerializeField]
    [Range(0.1f, 1f)]
    float duration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
