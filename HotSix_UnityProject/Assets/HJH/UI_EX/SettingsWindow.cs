using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = Vector2.zero;
    }

    public void Open(){
        transform.LeanScale(Vector2.one, 1.5f).setEaseOutBack();
    }
    public void Close(){
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }
}
