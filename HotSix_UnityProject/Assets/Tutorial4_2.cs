using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4_2 : MonoBehaviour
{
    private void Start(){
        transform.localScale = Vector2.zero;
    }
    public void StartSizing(){
        transform.LeanScale(Vector2.one, 1f).setEaseInOutCubic();
    }
}
