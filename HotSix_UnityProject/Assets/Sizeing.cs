using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sizeing : MonoBehaviour
{
    private void Start(){
        transform.localScale = new Vector2(0.8f,0.8f);
    }
    public void StartSizing(){
        transform.LeanScale(Vector2.one, 3f).setEaseInOutCubic().setLoopPingPong();
    }
}
