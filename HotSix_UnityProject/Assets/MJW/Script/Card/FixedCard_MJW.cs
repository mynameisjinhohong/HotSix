using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class FixedCard_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;

    public int id;
    public TextMeshProUGUI nameText;

    #endregion

    
    #region Methods

    private void OnEnable(){
        GetNameText();
    }

    public void GetNameText(){
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            nameText.text = gameManager.playerUnitTable.unitData[id].unitInfos.k_name;
        }
        else
        {
            nameText.text = gameManager.playerUnitTable.unitData[id].unitInfos.e_name;
        }
    }

    #endregion


    #region Monobehavior Callbacks

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
