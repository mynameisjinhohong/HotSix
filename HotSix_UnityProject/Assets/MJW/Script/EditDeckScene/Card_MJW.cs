using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card_MJW: MonoBehaviour
{
    public GameManager gameManager;
    public int id;
    public TextMeshProUGUI nameText;

    public void GetNameText(){
        nameText.text = gameManager.unitPrefabManager.unitPrefabs[id].GetComponent<UnitObject_MJW>().unit.unitInfo.k_name;
    }

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
    }
}
