using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCostManager_MJW : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI[] texts;
    public Deck_MJW currentDeck;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentDeck = gameManager.currentDeck;

        int count = transform.childCount;
        texts = new TextMeshProUGUI[count];
        for(int i = 0; i < count; ++i){
            texts[i] = transform.GetChild(i).Find("CostText").GetComponent<TextMeshProUGUI>();
        }

        for(int i = 0; i < count; ++i){
            int cost = gameManager.unitPrefabManager.unitPrefabs[currentDeck.unitIDs[i]].GetComponent<UnitObject_MJW>().unit.unitStat.cost;
            texts[i].text = cost.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
