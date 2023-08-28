using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5_3 : MonoBehaviour
{
    public CanvasGroup star1;
    public CanvasGroup star2;

    private void OnEnable(){
        star1.alpha = 0;
        star2.alpha = 0;
        star1.LeanAlpha(1,0.8f);
        star2.LeanAlpha(1,1.2f).delay = 0.8f;
    }
}
