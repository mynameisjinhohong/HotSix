using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemDown_HJH : MonoBehaviour
{
    public Menu_HJH menu;

    // Update is called once per frame
    void Update()
    {
        if (menu.gameEnd)
        {
            gameObject.SetActive(false);
        }
    }
}
