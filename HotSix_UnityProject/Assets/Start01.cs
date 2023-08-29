using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start01 : MonoBehaviour
{
    public Transform star1;
    public Transform star2;




    private void OnEnable(){
        star1.localPosition = new Vector2(Screen.width, 202f);
        star1.LeanMoveLocalX(0,1f).setEaseOutBack().delay = 0.5f;

        star2.localPosition = new Vector2(Screen.width,-380f);
        star2.LeanMoveLocalX(0,1f).setEaseOutBack().delay = 1.5f;
    }
}
