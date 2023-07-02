using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StagePopUpManager_MJW : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI[] texts;
    public Deck_MJW currentDeck;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentDeck = gameManager.currentDeck;
        for(int i = 0; i < 8; ++i){
            texts[i].text = gameManager.unitPrefabManager.unitPrefabs[currentDeck.unitIDs[i]].GetComponent<UnitObject_MJW>().unit.unitInfo.k_name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
