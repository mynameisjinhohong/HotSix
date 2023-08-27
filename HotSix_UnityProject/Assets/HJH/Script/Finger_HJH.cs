using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger_HJH : MonoBehaviour
{
    public float speed = 1f;
    public float time = 0.5f;
    float currentTime = 0;
    bool up = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (up)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if(currentTime > time)
        {
            if (up)
            {
                up = false;
            }
            else
            {
                up = true;
            }
            currentTime = 0;
        }
    }
}
