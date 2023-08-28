using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager_HJH : MonoBehaviour
{
    public EnemySpawnManager_MJW enemySpawnManager;

    public TMP_Text moneyText;
    public int moneyAmount;//�÷��� ���߿� ���� �� �Ӵ�
    public int maxMoney;
    public int startMoney;
    int Money;
    public AudioSource fullMoneyAudio;
    public int money
    {
        get
        {
            return Money;
        }
        set 
        {
            if (value > maxMoney)
            {
                if(Money < maxMoney)
                {
                    moneyAmount += maxMoney - Money;
                }
                Money = maxMoney;
                fullMoneyAudio.Play();
            }
            else
            {
                if (value > Money)
                {
                    moneyAmount += value - Money;
                }
                Money = value;
            }
        }
    }
    public int answerMoney;
    public int reduceMoney;
    public int timeMoney; // �ð��� �þ�� ��
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager_MJW>();
        money = startMoney;
        enemySpawnManager.totalMoney += startMoney;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 1f)
        {
            money += timeMoney;
            enemySpawnManager.totalMoney += timeMoney;
            currentTime = 0;
        }

        moneyText.text = money.ToString() + "   /   " + maxMoney.ToString();  
    }

    public void GetMoney(int wrongTry)
    {
        money += answerMoney - (reduceMoney * wrongTry);
        enemySpawnManager.totalMoney += answerMoney - (reduceMoney * wrongTry);
    }
}
