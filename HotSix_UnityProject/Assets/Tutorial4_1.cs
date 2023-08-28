using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4_1 : MonoBehaviour
{
    public CanvasGroup star1;
    public CanvasGroup star2;
    public CanvasGroup star3;
    public Transform box;

    private void Start(){
        box.localScale = new Vector2(0.8f,0.8f);
    }

    private void OnEnable(){
        star1.alpha = 0;
        star2.alpha = 0;    
        star3.alpha = 0;

        star1.LeanAlpha(1,0.8f).setEaseOutQuart();
        star2.LeanAlpha(1,1f).setEaseOutQuart().delay = 0.8f;
        star3.LeanAlpha(1,1f).setEaseOutQuart().delay = 0.8f;
        box.LeanScale(Vector2.one, 2f).setEaseInOutCubic().setLoopPingPong().delay = 1.6f;

    }
}
