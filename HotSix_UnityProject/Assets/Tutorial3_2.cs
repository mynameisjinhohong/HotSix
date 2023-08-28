using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3_2 : MonoBehaviour
{
    public CanvasGroup star1;
    public CanvasGroup star2;
    public CanvasGroup star3;  
    public CanvasGroup star4;

    private void OnEnable(){
        star1.alpha = 0;
        star2.alpha = 0;
        star3.alpha = 0;    
        star4.alpha = 0;
        star1.LeanAlpha(1,0.8f).setEaseOutQuart();
        star2.LeanAlpha(1,2f).setEaseOutQuart().delay = 0.8f;
        star3.LeanAlpha(1,2f).setEaseOutQuart().delay = 0.8f;
        star4.LeanAlpha(1,2f).setEaseOutQuart().delay = 2f;
    }
}
