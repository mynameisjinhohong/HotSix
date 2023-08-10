using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeckTextManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public TextMeshProUGUI[] texts;
    public Deck_MJW currentDeck;

    #endregion


    #region Methods

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        currentDeck = gameManager.currentDeck;
        for (int i = 0; i < 5; ++i)
        {
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                texts[i].text = gameManager.playerUnitTable.unitData[currentDeck.unitIDs[i]].entityInfos.k_name;
            }
            else
            {
                texts[i].text = gameManager.playerUnitTable.unitData[currentDeck.unitIDs[i]].entityInfos.e_name;
            }
        }
    }

    #endregion


    #region Monobehavior Callbacks

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
