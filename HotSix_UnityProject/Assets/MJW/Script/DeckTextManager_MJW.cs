using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeckTextManager_MJW : MonoBehaviour
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
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                texts[i].text = gameManager.unitPrefabManager.unitPrefabs[currentDeck.unitIDs[i]].GetComponent<UnitObject_MJW>().unit.unitInfo.k_name;
            }
            else
            {
                texts[i].text = gameManager.unitPrefabManager.unitPrefabs[currentDeck.unitIDs[i]].GetComponent<UnitObject_MJW>().unit.unitInfo.e_name;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
