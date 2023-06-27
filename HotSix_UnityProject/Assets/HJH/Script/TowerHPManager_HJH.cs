using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHPManager_HJH : MonoBehaviour
{
    [Header("슬라이더들")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;


    public int startPlayerTowerHP; // 플레이터 타워 최초 체력
    public int startEnemyTowerHP; //적 타워 최초 체력

    public int playerMaxHP; //플레이어 최대 체력
    public int enemyMaxHP;

    public int playerTowerHP; //플레이여 현재 체력
    public int enemyTowreHP;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHP = startEnemyTowerHP;
        enemyMaxHP = startEnemyTowerHP;
        playerTowerHP = startPlayerTowerHP;
        enemyTowreHP = startEnemyTowerHP;
    }

    // Update is called once per frame
    void Update()
    {
        playerHPSlider.value = (float)playerTowerHP / (float)playerMaxHP;
        enemyHPSlider.value = (float)enemyTowreHP / (float)enemyMaxHP;
    }
}
