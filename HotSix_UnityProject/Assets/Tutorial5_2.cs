using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5_2 : MonoBehaviour
{
    public Transform star1;
    public Transform star2;
    public Transform star3;
    public Transform star4;




    private void OnEnable(){
        star1.localPosition = new Vector2(0,-Screen.height);
        star1.LeanMoveLocalY(0,0.7f).setEaseOutExpo();

        star2.localPosition = new Vector2(0,-Screen.height);
        star2.LeanMoveLocalY(0,0.7f).setEaseOutExpo().delay = 0.7f;

        star3.localPosition = new Vector2(0,-Screen.height);
        star3.LeanMoveLocalY(-20f,0.7f).setEaseOutExpo().delay = 1.4f;

        star4.localPosition = new Vector2(0,-Screen.height);
        star4.LeanMoveLocalY(0,1.3f).setEaseOutBack().delay = 2.1f;



    }
}
