using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager_HJH : MonoBehaviour
{
    public TMP_Text moneyText;
    public int maxMoney;
    public int startMoney;
    public int money;
    public int answerMoney;
    public int reduceMoney;
    public int timeMoney; // ½Ã°£´ç ´Ã¾î³ª´Â µ·
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        money = startMoney;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 1f)
        {
            money += timeMoney;
            currentTime = 0;
        }
        if (money > maxMoney)
        {
            money = maxMoney;
        }
        moneyText.text = money.ToString() + "   /   " + maxMoney.ToString();  
    }

    public void GetMoney(int wrongTry)
    {
        money += answerMoney - (reduceMoney * wrongTry);
    }
}
