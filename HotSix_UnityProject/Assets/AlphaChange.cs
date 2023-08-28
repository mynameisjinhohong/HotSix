using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChange : MonoBehaviour
{
    public CanvasGroup star1;
    public CanvasGroup star2;

    private void OnEnable(){
        star1.alpha = 0;
        star2.alpha = 0;
        star1.LeanAlpha(1,2f).setLoopPingPong();
        star2.LeanAlpha(1,2f).setLoopPingPong().delay = 2f;
    }
}
