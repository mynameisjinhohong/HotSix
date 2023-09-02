using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelButton_HJH : MonoBehaviour
{
    public GameObject[] panels;
    public void OnOneOffAll(GameObject OnObject)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if (panels[i].gameObject == OnObject)
            {
                OnObject.SetActive(true);
            }
            else
            {
                panels[i].gameObject.SetActive(false);
            }
        }
    }
}
