using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ProfileManager_HJH : MonoBehaviour
{
    public Image profileImage;
    public TMP_Text nameText;
    public TMP_Text stageProgress;
    public TMP_Text stageWinRate;
    public TMP_Text stageClearTimeAVGMin;
    public TMP_Text stageClearTimeAVGSec;
    public TMP_Text stageSolveMathProblem;
    public TMP_Text problemCorrectRate;
    public TMP_Text mathCoinAmount;
    public GameManager gameManager;

    [Header("프로필 버튼용")]
    public Image smallProfileImage;
    public TMP_Text smallProfileNameText;
    public TMP_Text starText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        SettingProfile();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    MoveScene();
        //}
    }
    private void OnEnable()
    {


    }
    public void SettingProfile()
    {
        //프로필 버튼 설정
        smallProfileImage.sprite = GameManager.instance.unitImages.playerUnitImages[gameManager.userData.porfileImg].proFile_Icon;
        smallProfileNameText.text = gameManager.userData.userName;
        int star = 0;
        for(int i =0; i < gameManager.userData.stageProgress; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (GameManager.instance.userData.stageStar[i+1].stageStar[i] == true)
                {
                    star++;
                }
            }
        }
        starText.text = star + "/" + gameManager.userData.stageProgress * 3;
        //프로필 켰을 때 창
        profileImage.sprite = GameManager.instance.unitImages.playerUnitImages[gameManager.userData.porfileImg].iconImage;
        nameText.text = gameManager.userData.userName;
        stageProgress.text = gameManager.userData.stageProgress.ToString();
        if ((float)(gameManager.userData.loseCount + gameManager.userData.winCount) > 0)
        {
            stageWinRate.text = ((float)gameManager.userData.winCount / (float)(gameManager.userData.loseCount + gameManager.userData.winCount) * 100).ToString() + "%";
        }
        else
        {
            stageWinRate.text = "0%";
        }
        if (gameManager.userData.winCount > 0)
        {
            int time = Mathf.CeilToInt((gameManager.userData.stageClearTime / (float)(gameManager.userData.winCount)));
            int min = time / 60;
            int sec = time % 60;
            stageClearTimeAVGMin.text = min.ToString();
            stageClearTimeAVGSec.text = sec.ToString();
        }
        else
        {
            stageClearTimeAVGMin.text = "0";
            stageClearTimeAVGSec.text = "0";
        }
        stageSolveMathProblem.text = gameManager.userData.solveCount.ToString();
        if (gameManager.userData.tryCount > 0)
        {
            problemCorrectRate.text = Mathf.Floor(((float)gameManager.userData.solveCount / (float)gameManager.userData.tryCount) * 10000) / 100 + "%";
        }
        else
        {
            problemCorrectRate.text = "0%";
        }
        mathCoinAmount.text = gameManager.userData.mathCoinAmount.ToString();
    }

    public void ProfileEditApply()
    {
        GameManager.instance.SaveUserData();
        SettingProfile();
    }

}
