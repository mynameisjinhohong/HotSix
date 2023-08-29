using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : MonoBehaviour
{       
    public Transform box;
    public GameObject after;
    public float waitTime;
    private void OnEnable(){
        box.localPosition = new Vector2(0,-Screen.height);
        box.LeanMoveLocalY(0,0.7f).setEaseOutBack();
        Invoke("TurnOn", waitTime);
    }
    public void TurnOn()
    {
        after.SetActive(true);
    }
}
