using KoreanTyper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager_HJH : MonoBehaviour
{
    public enum TutorialState
    {
        CutScene,
        InputTime,
        GameExplain,
        GamePlay,
        StageExplain,
        DeckEditExplain,
    }
    public TutorialState state = TutorialState.CutScene;
    public GameObject[] stateObject;
    [Header("컷신 관련")]
    public GameObject[] cutScenes;
    public TMP_Text[] cutSceneText;
    public float cutSceneSpeed = 0.03f;
    bool cutSceneTextEnd = true;
    int cutSceneIdx = 0;
    bool skip = false;
    public bool touchWait = false;
    float touchWaitTime = 0.5f;

    [Header("입력 관련")]
    public TMP_InputField nameInputField;
    public GameObject inputName;
    public GameObject inputLevel;
    public GameObject textBox;
    public Button confirmButton;

    [Header("게임 설명 관련")]
    public GameObject[] explainBubble;
    public GameObject[] explainImage;
    public Sprite[] englishSprite;
    public TMP_Text playerNameText;
    public int explainIdx = 0;

    [Header("게임 플레이 관련")]
    public bool gameClear = false;
    public CameraMove_HJH camera;
    public GameObject stageBG;
    [Header("스테이지 관련")]
    public GameObject[] profileFingers;
    public GameObject[] stageBubble;
    public StageButtonManager_MJW stageButton;
    [Header("덱 설명")]
    public GameObject[] deckBubble;
    public int deckIdx = 0;
    public GameObject dontClick;
    public EditDeckManager_MJW editDeck;
    public bool waitDrag = false;
    public GameObject upgradeFinger;
    [Header("마무리")]
    public GameObject lastMessage;
    public bool last = false;
    public GameObject stagePopup;
    public GameObject bubble;
    public Image backButton;
    // Start is called before the first frame update
    void Start()
    {
        
        if (GameManager.instance.tutorialRestart)
        {
            state = TutorialState.GamePlay;
            Time.timeScale = 1f;
            GameManager.instance.tutorialRestart = false;
            ChangeStateOnOff();
        }
        else
        {
            CutSceneOnOff(cutSceneIdx, cutScenes);
            TextOnOff(cutSceneIdx);
            GameManager.instance.bgm.clip = GameManager.instance.bgmSources[5];
            GameManager.instance.bgm.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (last && !touchWait)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlayerPrefs.SetInt("Tutorial", 1);
                LoadingManager_HJH.LoadScene("StageScene");
            }
        }
        if(state == TutorialState.CutScene)
        {
            if (Input.GetMouseButtonDown(0) && !touchWait)
            {
                if (cutSceneIdx >= cutScenes.Length - 1)
                {
                    state = TutorialState.InputTime;
                    textBox.SetActive(false);
                    ChangeStateOnOff();
                }
                else
                {
                    if (cutSceneTextEnd)
                    {
                        cutSceneIdx++;
                        CutSceneOnOff(cutSceneIdx, cutScenes);
                        TextOnOff(cutSceneIdx);
                    }
                    else
                    {
                        skip = true;
                    }
                }
                StartCoroutine(TouchWait());
            }
        }
        else if(state == TutorialState.InputTime)
        {

        }
        else if(state == TutorialState.GameExplain) 
        {
            if (Input.GetMouseButtonDown(0) && !touchWait)
            {
                explainIdx++;
                if (explainIdx > explainBubble.Length - 1)
                {
                    state = TutorialState.GamePlay;
                    ChangeStateOnOff();
                }
                else
                {
                    OnOff(explainIdx,explainBubble);
                    OnOff(explainIdx,explainImage);
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                    {
                        if (explainImage[explainIdx].GetComponent<Image>().sprite.name.Contains("1"))
                        {
                            explainImage[explainIdx].GetComponent<Image>().sprite = englishSprite[0];
                        }
                        else
                        {
                            explainImage[explainIdx].GetComponent<Image>().sprite = englishSprite[1];
                        }
                        
                    }
                }
                StopAllCoroutines();
                StartCoroutine(TouchWait());
            }
        }
        else if(state == TutorialState.GamePlay)
        {
            if (gameClear)
            {
                GameManager.instance.currentStage = null;
                GameManager.instance.gameState = GameManager.GameState.GamePlay;
                GameManager.instance.bgm.clip = GameManager.instance.bgmSources[0];
                GameManager.instance.bgm.Play();
                state = TutorialState.StageExplain;
                camera.background = stageBG;
                ChangeStateOnOff();
                camera.FirstSetting();
                stageButton.tutoOK = true;
            }
        }
        else if(state == TutorialState.StageExplain)
        {

        }
        else if(state == TutorialState.DeckEditExplain)
        {
            if (Input.GetMouseButtonDown(0) && !touchWait && !waitDrag)
            {
                deckIdx++;
                if (deckIdx > deckBubble.Length - 1)
                {
                    //state = TutorialState.GamePlay;
                    //ChangeStateOnOff();
                    //GameManager.instance.bgm.clip = GameManager.instance.bgmSources[1];
                }
                else
                {
                    OnOff(deckIdx, deckBubble);
                    if(deckIdx == 2)
                    {
                        editDeck.tutorial = false;
                        waitDrag = true;
                        dontClick.SetActive(true);
                    }
                    else if(deckIdx == 4)
                    {
                        waitDrag = true;
                        upgradeFinger.SetActive(true);
                    }
                    else if(deckIdx == 6)
                    {
                        backButton.raycastTarget = true;
                    }
                    
                }
                StopAllCoroutines();
                StartCoroutine(TouchWait());
            }
        }

    }
    void TextOnOff(int idx)
    {
        for(int i =0;i<cutSceneText.Length;i++)
        {
            if(i == idx)
            {
                cutSceneText[i].gameObject.SetActive(true);
                StartCoroutine(TextAni());
            }
            else
            {
                cutSceneText[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TextAni()
    {
        cutSceneTextEnd = false;
        float textScale = cutSceneText[cutSceneIdx].fontSize;
        cutSceneText[cutSceneIdx].enableAutoSizing = false;
        cutSceneText[cutSceneIdx].fontSize = textScale;
        string text = cutSceneText[cutSceneIdx].text;
        int typeLength = cutSceneText[cutSceneIdx].text.GetTypingLength();
        for (int i = 0; i < typeLength+1; i++)
        {
            cutSceneText[cutSceneIdx].text = text.Typing(i);
            if (skip)
            {
                skip = false;
                cutSceneText[cutSceneIdx].text = text;
                break;
            }
            yield return new WaitForSeconds(cutSceneSpeed);
        }
        cutSceneTextEnd = true;
    }

    void ChangeStateOnOff()
    {
        for(int i = 0; i<stateObject.Length;i++)
        {
            if(i == (int)state)
            {
                stateObject[i].SetActive(true);
            }
            else
            {
                stateObject[i].SetActive(false);
            }
        }
    }
    void OnOff(int idx, GameObject[] things)
    {
        for (int i = 0; i < things.Length; i++)
        {
            if (i == idx)
            {
                things[i].gameObject.SetActive(true);

            }
            else
            {
                things[i].gameObject.SetActive(false);
            }
        }
    }
    void CutSceneOnOff(int idx, GameObject[] things)
    {
        GameObject now = things[idx].gameObject;
        for (int i = 0; i < things.Length; i++)
        {
            if (i == idx)
            {
                things[i].gameObject.SetActive(true);

            }
            else
            {
                if (things[i] != now)
                {
                    things[i].gameObject.SetActive(false);
                }
            }
        }
    }
    IEnumerator TouchWait()
    {
        touchWait = true;
        yield return new WaitForSeconds(touchWaitTime);
        touchWait = false;
    }
    public void InputName()
    {
        string Name = nameInputField.text;
        if(Name.Length < 1)
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
            {
                Name = "수학 대장";
            }
            else
            {
                Name = "Math Captin";
            }
        }
        GameManager.instance.userData.userName = Name;
        playerNameText.text = Name;
        inputName.SetActive(false);
        inputLevel.SetActive(true);
    }
    public void ChooseDifficulty(int difficult)
    {
        GameManager.instance.userData.userLevel = difficult;
        state = TutorialState.GameExplain;
        ChangeStateOnOff();
        GameManager.instance.bgm.clip = GameManager.instance.bgmSources[1];
        GameManager.instance.bgm.Play();
    }

    public void Restart()
    {
        GameManager.instance.tutorialRestart = true;
        LoadingManager_HJH.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ProfileButton()
    {
        OnOff(1, stageBubble);
        OnOff(1, profileFingers);
    }

    public void ProfileOffButton()
    {
        OnOff(2, profileFingers);
        OnOff(2, stageBubble);
        GameManager.instance.currentStage = null;
        camera.isActive = false;
        stageButton.tutoOK = false;
        camera.gameObject.transform.position = new Vector3(camera.startPoint, 0, -10); ;
    }
    public void StageClick()
    {
        OnOff(3,profileFingers);
        OnOff(3,stageBubble);
    }

    public void DeckEdit()
    {
        editDeck.tutorial = true;
        state = TutorialState.DeckEditExplain;
        ChangeStateOnOff();
    }
    public void DragDeck()
    {
        waitDrag = false;
        editDeck.tutorial = true;
        dontClick.SetActive(false);
        deckIdx++;
        OnOff(deckIdx, deckBubble);
    }

    public void UpGrade()
    {
        waitDrag = false;
        upgradeFinger.SetActive(false);
        deckIdx++;
        OnOff(deckIdx, deckBubble);
    }

    public void BackToStage()
    {
        state = TutorialState.StageExplain;
        ChangeStateOnOff();
        lastMessage.SetActive(true);
        last = true;
        stagePopup.SetActive(false);
        bubble.SetActive(false);
        StartCoroutine(TouchWait());
    }

    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        Time.timeScale = 1;
        GameManager.instance.currentStage = null;
        LoadingManager_HJH.LoadScene("StageScene");
    }
}
