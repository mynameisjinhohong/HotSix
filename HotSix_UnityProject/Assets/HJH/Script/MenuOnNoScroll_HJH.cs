using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOnNoScroll_HJH : MonoBehaviour
{

    private void OnEnable()
    {
        GameManager.instance.gameState = GameManager.GameState.GameStop;
    }
    private void OnDisable()
    {
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
    }
}
