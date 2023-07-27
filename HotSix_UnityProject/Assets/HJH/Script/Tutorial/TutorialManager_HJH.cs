using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager_HJH : MonoBehaviour
{
    enum TutorialState
    {
        CutScene,
        InputTime,
        GameExplain,
        StageExplain,
    }
    TutorialState state = TutorialState.CutScene;
    public GameObject[] stateObject;
    [Header("컷신 관련")]
    public Sprite[] cutScenes;
    public Image cutSceneImage;
    public TMP_Text[] cutSceneText;
    int cutSceneIdx = 0;
    bool touchWait = false;
    float touchWaitTime = 0.5f;
    [Header("입력 관련")]
    public TMP_InputField nameInputField;
    public GameObject inputName;
    public GameObject inputLevel;


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
                    Debug.Log("ii");
                }
                else
                {
                    cutSceneImage.sprite = cutScenes[cutSceneIdx];
                    TextOnOff(cutSceneIdx);
                }
                StartCoroutine(TouchWait());
            }
        }
        else if(state == TutorialState.InputTime)
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
    IEnumerator TouchWait()
    {
        touchWait = true;
        yield return new WaitForSeconds(touchWaitTime);
        touchWait = false;
    }
    void InputName(string name)
    {
        GameManager.instance.userData.userName = name;
        inputName.SetActive(false);
        inputLevel.SetActive(true);
    }
    public void ChooseDifficulty(int difficult)
    {
        GameManager.instance.userData.userLevel = difficult;
        state = TutorialState.GameExplain;
    }
}
