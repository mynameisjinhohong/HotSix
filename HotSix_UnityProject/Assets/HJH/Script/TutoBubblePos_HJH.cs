using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBubblePos_HJH : MonoBehaviour
{
    public GameObject bubble;
    public Vector3 moveVec;
    void Update()
    {
        bubble.transform.position = Camera.main.WorldToScreenPoint(transform.position) + moveVec;

        
    }
}
