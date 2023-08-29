using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5_1 : MonoBehaviour
{
    public CanvasGroup star1;
    public float waitTime;
    public float waitTime2;
    public GameObject second;
    public GameObject third;
    private void OnEnable(){
        star1.alpha = 0;

        star1.LeanAlpha(1,1.2f);
        Invoke("SecondRun", waitTime);
    }
    public void SecondRun()
    {
        second.SetActive(true);
        Invoke("ThirdRun", waitTime2);
    }
    public void ThirdRun()
    {
        third.SetActive(true);
    }
}
