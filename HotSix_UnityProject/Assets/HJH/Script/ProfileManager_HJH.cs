using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ProfileManager_HJH : MonoBehaviour
{
    public Image profileImage;
    public Sprite[] images;
    public TMP_Text nameText;
    public TMP_Text stageProgress;
    public TMP_Text stageWinRate;
    public TMP_Text stageClearTimeAVG;
    public TMP_Text stageSolveMathProblem;
    public TMP_Text problemCorrectRate;
    public TMP_Text mathCoinAmount;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        profileImage.sprite = images[gameManager.userData.porfileImg];
        nameText.text = gameManager.userData.userName;
        stageProgress.text = "스테이지 진행도 : " + gameManager.userData.staageProgress.ToString();
        if((float)(gameManager.userData.loseCount + gameManager.userData.winCount) > 0)
        {
            stageWinRate.text = "스테이지 승률 : " +  ((float)gameManager.userData.winCount / (float)(gameManager.userData.loseCount + gameManager.userData.winCount) * 100).ToString() +"%";
        }
        else
        {
            stageWinRate.text = "스테이지 승률 : 0%";
        }
        if(gameManager.userData.winCount > 0)
        {
            stageClearTimeAVG.text = "평균 클리어시간 : " +(gameManager.userData.stageClearTime / (float)(gameManager.userData.winCount)).ToString();
        }
        else
        {
            stageClearTimeAVG.text = "평균 클리어시간 : 0";
        }
        stageSolveMathProblem.text = "푼 전체 수학문제 수 : " +gameManager.userData.solveCount.ToString();
        if (gameManager.userData.tryCount >0)
        {
            problemCorrectRate.text = "평균 문제 풀이 정답률 : " + (((float)gameManager.userData.solveCount/(float)gameManager.userData.tryCount) * 100).ToString() +"%";
        }
        else
        {
            problemCorrectRate.text = "평균 문제 풀이 정답률 : 0%";
        }
        mathCoinAmount.text = "얻은 메스코인량 : " +gameManager.userData.mathCoinAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveScene();
        }
    }

    public void MoveScene()
    {
            SceneManager.LoadScene("StageScene");
    }

}
