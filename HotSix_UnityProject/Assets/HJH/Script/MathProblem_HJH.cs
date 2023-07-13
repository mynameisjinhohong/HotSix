using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathProblem_HJH : MonoBehaviour
{
    [SerializeField] WJ_Connector wj_conn;
    [SerializeField] CurrentStatus currentStatus;
    public CurrentStatus CurrentStatus => currentStatus;

    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //난이도 선택 패널
    [SerializeField] GameObject panel_question;         //문제 패널(진단,학습)

    [SerializeField] Text textDescription;        //문제 설명 텍스트
    [SerializeField] TEXDraw textEquation;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[] btAnsr = new Button[4]; //정답 버튼들
    TEXDraw[] textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("For Debug")]
    [SerializeField] WJ_DisplayText wj_displayText;         //텍스트 표시용(필수X)
    [SerializeField] Button getLearningButton;      //문제 받아오기 버튼

    [Header("Money")]
    public MoneyManager_HJH moneyManager;
    int wrongTry = 0; //틀린 횟수
    public int firstMoney; //문제 바로 맞췄을 때 받는 돈
    public int reduceMoney; // 문제 틀렸을 때 감소되는양

    public TMP_InputField answerInputField;

    public LaneManager_MJW laneManager;

    [Header("WrongAnwerCheck")]
    [Range(0f,10f)]
    public float wrongAnswerCheckTime = 1f;
    public int wrongAnswerCheckAmount = 3;
    public GameObject alertPopUp;
    bool wrongAnswerChecking = false;
    public int wrongAlert = 0;

    [Header("AudioSources")]
    public AudioSource wrongAudio;
    public AudioSource correctAudio;

    #region 플레이어 정보 저장에 필요한 것들
    int tryCount = 0; //정답을 맞추려 시도한 횟수
    int correctCount = 0; // 푼 문제 수
    public void SaveData()
    {
        Debug.Log(correctCount);
        GameManager.instance.userData.solveCount += correctCount;
        GameManager.instance.userData.tryCount += tryCount;
    }
    #endregion

    private void Awake()
    {
        //Debug.Log("1");
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)

            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("대기중", "", "", "");
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        switch (currentStatus)
        {
            case CurrentStatus.WAITING:
                panel_diag_chooseDiff.SetActive(true);
                break;
        }

        if (wj_conn != null)
        {
            wj_conn.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_conn.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
        answerInputField.onSubmit.AddListener(WriteAnswer);
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
        if(wrongTry == 1 && !wrongAnswerChecking)
        {
            StartCoroutine(WrongAnswerCheck());
        }
    }
    IEnumerator WrongAnswerCheck() //연속으로 틀리는거 검사하는 코루틴
    {
        wrongAnswerChecking = true;
        float currentTime = 0;
        int wrong = 0;
        int wrongtry = wrongTry; //이전 틀린 개수
        while(currentTime < wrongAnswerCheckTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            
            if(wrongtry != wrongTry)
            {
                if(wrongTry != 0)
                {
                    wrong++;
                }
                wrongtry = wrongTry;
            }
            if(wrong >= wrongAnswerCheckAmount)
            {
                if(wrongAlert == 0)
                {
                    alertPopUp.SetActive(true);
                    wrongAnswerChecking = false;
                    wrongAlert++;
                    break;
                }
                else if(wrongAlert == 1)
                {
                    wrongAnswerChecking = false;
                    laneManager.SpawnEnemyUnit(0, 9);
                    wrongAlert = 0;
                    break;
                }
            }
        }
        wrongAnswerChecking = false;
        Debug.Log("TimeOver");
    }

    /// <summary>
    /// 진단평가 문제 받아오기
    /// </summary>
    private void GetDiagnosis()
    {
        switch (wj_conn.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(wj_conn.cDiagnotics.data.textCn,
                            wj_conn.cDiagnotics.data.qstCn,
                            wj_conn.cDiagnotics.data.qstCransr,
                            wj_conn.cDiagnotics.data.qstWransr);
                wj_displayText.SetState("진단평가 중", "", "", "");
                break;
            case "E":
                Debug.Log("진단평가 끝! 학습 단계로 넘어갑니다.");
                wj_displayText.SetState("진단평가 완료", "", "", "");
                currentStatus = CurrentStatus.LEARNING;
                getLearningButton.interactable = true;
                wj_conn.Learning_GetQuestion();
                break;
        }
    }

    /// <summary>
    ///  n 번째 학습 문제 받아오기
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0) currentQuestionIndex = 0;

        MakeQuestion(wj_conn.cLearnSet.data.qsts[_index].textCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCransr,
                    wj_conn.cLearnSet.data.qsts[_index].qstWransr);
    }

    /// <summary>
    /// 받아온 데이터를 가지고 문제를 표시
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string correctAnswer;
        string[] wrongAnswers;

        int ran = Random.Range(0, 5);

        if (int.TryParse(qstCransr, out int result) == false) //정답이 정수가 아니면 무조건 버튼형태로 나오게
        {
            ran = 1;
        }
        textDescription.text = textCn;
        textEquation.text = qstCn;
        correctAnswer = qstCransr;
        wrongAnswers = qstWransr.Split(',');
        if (ran != 0)
        {

            int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;
            answerInputField.gameObject.SetActive(false);
            for (int i = 0; i < btAnsr.Length; i++)
            {
                if (i < ansrCount)
                    btAnsr[i].gameObject.SetActive(true);
                else
                    btAnsr[i].gameObject.SetActive(false);
            }

            int ansrIndex = Random.Range(0, ansrCount);

            for (int i = 0, q = 0; i < ansrCount; ++i, ++q)
            {
                if (i == ansrIndex)
                {
                    textAnsr[i].text = correctAnswer;
                    --q;
                }
                else
                    textAnsr[i].text = wrongAnswers[q];
            }
            isSolvingQuestion = true;
        }
        else
        {
            for(int i = 0; i< btAnsr.Length; ++i)
            {
                btAnsr[i].gameObject.SetActive(false);
            }
            answerInputField.gameObject.SetActive(true);
            isSolvingQuestion = true;
        }

    }

    //인풋필드로 답 넣었을 때
    public void WriteAnswer(string answer)
    {
        bool isCorrect;
        string ansrCwYn = "N";
        tryCount += 1;
        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                isCorrect = answer.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";
                if (isCorrect)
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(answer, ansrCwYn, (int)(questionSolveTime * 1000));
                    wj_displayText.SetState("진단평가 중", answer, ansrCwYn, questionSolveTime + " 초");
                    panel_question.SetActive(false);
                    questionSolveTime = 0;
                }
                else
                {
                    wrongAudio.Play();
                    wrongTry += 1;
                }
                break;
            case CurrentStatus.LEARNING:
                isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                if (ansrCwYn == "Y")
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, answer, ansrCwYn, (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("문제풀이 중", answer, ansrCwYn, questionSolveTime + " 초");

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
                        wj_displayText.SetState("문제풀이 완료", "", "", "");
                        wj_conn.Learning_GetQuestion();
                    }
                    else GetLearning(currentQuestionIndex);

                    questionSolveTime = 0;
                }
                else if (ansrCwYn == "N")
                {
                    wrongAudio.Play();
                    wrongTry += 1;
                }
                break;
        }
        answerInputField.text = "";
        
    }

    /// <summary>
    /// 답을 고르고 맞았는 지 체크
    /// </summary>
    public void SelectAnswer(int _idx)
    {
        bool isCorrect;
        string ansrCwYn = "N";
        tryCount += 1;
        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";
                if(ansrCwYn == "Y")
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));
                    wj_displayText.SetState("진단평가 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");
                    panel_question.SetActive(false);
                    questionSolveTime = 0;
                }
                else if(ansrCwYn == "N")
                {
                    wrongAudio.Play();
                    wrongTry += 1;
                    btAnsr[_idx].transform.gameObject.SetActive(false);
                }
                break;

            case CurrentStatus.LEARNING:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                if(ansrCwYn == "Y")
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("문제풀이 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
                        wj_displayText.SetState("문제풀이 완료", "", "", "");
                        wj_conn.Learning_GetQuestion();
                    }
                    else GetLearning(currentQuestionIndex);

                    questionSolveTime = 0;
                }
                else if (ansrCwYn == "N")
                {
                    wrongAudio.Play();
                    wrongTry += 1;
                    btAnsr[_idx].transform.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        wj_conn.FirstRun_Diagnosis(a);
    }
    public void ButtonEvent_GetLearning()
    {
        wj_conn.Learning_GetQuestion();
        wj_displayText.SetState("문제풀이 중", "-", "-", "-");
    }
    #endregion
}
