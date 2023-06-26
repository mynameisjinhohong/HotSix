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
    // Start is called before the first frame update
    void Start()
    {
        money = startMoney;
    }

    // Update is called once per frame
    void Update()
    {
        if (money > maxMoney)
        {
            money = maxMoney;
        }
        moneyText.text = money.ToString() + "   /   " + maxMoney.ToString();  
    }
}
