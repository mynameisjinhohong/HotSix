using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChange2 : MonoBehaviour
{
    public CanvasGroup star1;


    private void OnEnable(){
        star1.alpha = 0;

        star1.LeanAlpha(1,2f).setLoopPingPong();

    }
}
