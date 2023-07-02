using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitManager_MJW : MonoBehaviour
{
    [HideInInspector]
    public GameObject quitPopUp;

    // Start is called before the first frame update
    void Start()
    {
        quitPopUp = transform.Find("QuitPopUp").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(quitPopUp.activeSelf){
                DisablePopUP();
            }
            else{
                EnablePopUp();
            }
        }
    }

    public void EnablePopUp(){
        quitPopUp.SetActive(true);
    }

    public void DisablePopUP(){
        quitPopUp.SetActive(false);
    }

    public void Quit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}