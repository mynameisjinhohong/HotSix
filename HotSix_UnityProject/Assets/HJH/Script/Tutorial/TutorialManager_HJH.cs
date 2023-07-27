using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    }
    public TutorialState state = TutorialState.CutScene;
    public GameObject[] stateObject;
    [Header("컷신 관련")]
    public Sprite[] cutScenes;
    public Image cutSceneImage;
    public TMP_Text[] cutSceneText;
    int cutSceneIdx = 0;
    public bool touchWait = false;
    float touchWaitTime = 0.5f;

    [Header("입력 관련")]
    public TMP_InputField nameInputField;
    public GameObject inputName;
    public GameObject inputLevel;

    [Header("게임 설명 관련")]
    public GameObject[] explainBubble;
    public GameObject[] explainImage;
    public TMP_Text playerNameText;
    public GameObject towerUpgradeButton;
    public int explainIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        nameInputField.onEndEdit.AddListener(InputName);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == TutorialState.CutScene)
        {
            if (Input.GetMouseButtonDown(0) && !touchWait)
            {
                cutSceneIdx++;
                if (cutSceneIdx > cutScenes.Length - 1)
                {
                    state = TutorialState.InputTime;
                    ChangeStateOnOff();
                }
                else
                {
                    cutSceneImage.sprite = cutScenes[cutSceneIdx];
                    TextOnOff(cutSceneIdx);
                }
                StopAllCoroutines();
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
                }
                StopAllCoroutines();
                StartCoroutine(TouchWait());
            }
        }
        else if(state == TutorialState.GamePlay)
        {

        }

    }
    void TextOnOff(int idx)
    {
        for(int i =0;i<cutSceneText.Length;i++)
        {
            if(i == idx)
            {
                cutSceneText[i].gameObject.SetActive(true);

            }
            else
            {
                cutSceneText[i].gameObject.SetActive(false);
            }
        }
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
    IEnumerator TouchWait()
    {
        touchWait = true;
        yield return new WaitForSeconds(touchWaitTime);
        touchWait = false;
    }
    void InputName(string name)
    {
        GameManager.instance.userData.userName = name;
        playerNameText.text = name;
        inputName.SetActive(false);
        inputLevel.SetActive(true);
    }
    public void ChooseDifficulty(int difficult)
    {
        GameManager.instance.userData.userLevel = difficult;
        state = TutorialState.GameExplain;
        ChangeStateOnOff();
    }
}
