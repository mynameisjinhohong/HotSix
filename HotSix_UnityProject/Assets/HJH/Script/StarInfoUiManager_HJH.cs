using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StarInfoUiManager_HJH : MonoBehaviour
{
    public GameObject[] starParents;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        int stageNum = (int)GameManager.instance.currentStage;
        for (int i = 0; i < 3; i++)
        {
            bool clear = GameManager.instance.userData.stageStar[stageNum].stageStar[i];
            int condition = GameManager.instance.starCondition[stageNum].whatIsCondition[i];
            SetStarCondition(stageNum,i, condition, clear);
        }
    }

    public void SetStarCondition(int stage,int what, int condition, bool clear) //what = 몇번째 게임오브젝트(밑에서 부터 0,1,2) condition = 어떤 조건인지, clear = 이전에 star를 얻었던 것인지
    {
        if (clear)
        {
            starParents[what].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = GameManager.instance.starImage[1];
        }
        else
        {
            starParents[what].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = GameManager.instance.starImage[0];
        }
        TMP_Text text = starParents[what].transform.GetChild(1).GetComponent<TMP_Text>();
        switch (condition)
        {
            case 0:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    text.text = "Stage Clear";
                }
                else
                {
                    text.text = "스테이지 클리어";
                }
                break;
            case 1:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) 
                {
                    text.text = "Clear in " + GameManager.instance.starCondition[stage].gameClearTime / 60 + ":" + GameManager.instance.starCondition[stage].gameClearTime % 60 + "sec";
                }
                else
                {
                    text.text = GameManager.instance.starCondition[stage].gameClearTime/60 +":" + GameManager.instance.starCondition[stage].gameClearTime%60 + "이하로 클리어";
                }
                break;
            case 2:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) 
                {
                    text.text = "Use less than " + GameManager.instance.starCondition[stage].gameClearTime + "mathcoin";
                }
                else
                {
                    text.text = "메스코인"+GameManager.instance.starCondition[stage].mathCoinAmount + "미만 사용";
                }
                break;
                case 3:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    text.text = "Tower Hp Over " + GameManager.instance.starCondition[stage].playrTowerHpPercentage + "%";
                }
                else
                {
                    text.text = "성 체력" + GameManager.instance.starCondition[stage].playrTowerHpPercentage + "% 이상";
                }
                break;
                case 4:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    text.text = "More than " + GameManager.instance.starCondition[stage].mathProblemCorrectness + "% correct answer rate";
                }
                else
                {
                    text.text = "문제 정답률" + GameManager.instance.starCondition[stage].mathProblemCorrectness + "% 이상";
                }
                break;
            case 5:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    text.text = "Less than " + GameManager.instance.starCondition[stage].playerTowerUpgrade + "tower upgrade";
                }
                else
                {
                    text.text = "성 업그레이드 " + (GameManager.instance.starCondition[stage].playerTowerUpgrade) + " 이상";
                }
                break;
        }
        starParents[what].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.rewardData[stage].startCardAmount[0].ToString();

    }
}
