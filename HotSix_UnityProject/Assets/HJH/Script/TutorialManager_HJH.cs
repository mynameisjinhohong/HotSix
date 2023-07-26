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
    public Sprite[] cutScenes;
    public Image cutSceneImage;
    public string[] cutSceneTexts;
    public TMP_Text cutSceneText;
    int cutSceneIdx = 0;
    bool touchWait = false;
    float touchWaitTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !touchWait&& state == TutorialState.CutScene)
        {
            cutSceneIdx++;
            if(cutSceneIdx > cutScenes.Length-1)
            {
                state = TutorialState.InputTime;
            }
            else
            {
                cutSceneImage.sprite = cutScenes[cutSceneIdx];
                cutSceneText.text = cutSceneTexts[cutSceneIdx];
            }
            StartCoroutine(TouchWait());
        }
    }

    IEnumerator TouchWait()
    {
        touchWait = true;
        yield return new WaitForSeconds(touchWaitTime);
        touchWait = false;
    }
}
