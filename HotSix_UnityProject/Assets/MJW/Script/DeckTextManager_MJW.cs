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

    }
    private void OnEnable()
    {
        gameManager = GameManager.instance;
        currentDeck = gameManager.currentDeck;
        for (int i = 0; i < 8; ++i)
        {
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                texts[i].text = gameManager.playerUnitTable.unitData[currentDeck.unitIDs[i]].unitInfos.k_name;
            }
            else
            {
                texts[i].text = gameManager.playerUnitTable.unitData[currentDeck.unitIDs[i]].unitInfos.e_name;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
