using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHPManager_HJH : MonoBehaviour
{
    [Header("슬라이더들")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;


    public float startPlayerTowerHP; // 플레이터 타워 최초 체력
    public float startEnemyTowerHP; //적 타워 최초 체력

    public float playerMaxHP; //플레이어 최대 체력
    public float enemyMaxHP;

    public float playerTowerHP; //플레이여 현재 체력
    public float enemyTowerHP;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHP = startEnemyTowerHP;
        enemyMaxHP = startEnemyTowerHP;
        playerTowerHP = startPlayerTowerHP;
        enemyTowerHP = startEnemyTowerHP;
    }

    // Update is called once per frame
    void Update()
    {
        playerHPSlider.value = playerTowerHP / playerMaxHP;
        enemyHPSlider.value = enemyTowerHP / enemyMaxHP;
        if(playerTowerHP <= 0)
        {
            Debug.Log("GameOVer");
        }
        if(enemyTowerHP <= 0)
        {
            Debug.Log("GaemClear");
        }
    }
}
