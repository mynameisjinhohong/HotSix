using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System;

public class MathProblem_HJH : MonoBehaviour
{
    [SerializeField] WJ_Connector wj_conn;
    [SerializeField] CurrentStatus currentStatus;
    public CurrentStatus CurrentStatus => currentStatus;

    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //���̵� ���� �г�
    [SerializeField] GameObject panel_question;         //���� �г�(����,�н�)

    [SerializeField] Text textDescription;        //���� ���� �ؽ�Ʈ
    [SerializeField] TEXDraw textEquation;           //���� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)
    [SerializeField] Button[] btAnsr = new Button[4]; //���� ��ư��
    TEXDraw[] textAnsr;                  //���� ��ư�� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;


    [Header("Money")]
    public MoneyManager_HJH moneyManager;
    int wrongTry = 0; //Ʋ�� Ƚ��
    public int firstMoney; //���� �ٷ� ������ �� �޴� ��
    public int reduceMoney; // ���� Ʋ���� �� ���ҵǴ¾�

    public TMP_InputField answerInputField;

    public EnemySpawnManager_MJW enemySpawnManager;

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

    #region �÷��̾� ���� ���忡 �ʿ��� �͵�
    public int tryCount = 0; //������ ���߷� �õ��� Ƚ��
    public int correctCount = 0; // Ǭ ���� ��
    public void SaveData()
    {
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
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {


        if (wj_conn != null)
        {
            wj_conn.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_conn.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
        answerInputField.onSubmit.AddListener(WriteAnswer);
        //switch (currentStatus)
        //{
        //    case CurrentStatus.WAITING:
        //        panel_diag_chooseDiff.SetActive(true);
        //        break;
        //}
        Invoke("NextSetUp", 0.1f);
    }
    void NextSetUp()
    {
        ButtonEvent_ChooseDifficulty(GameManager.instance.userData.userLevel);
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
        if(wrongTry == 1 && !wrongAnswerChecking)
        {
            StartCoroutine(WrongAnswerCheck());
        }
    }
    IEnumerator WrongAnswerCheck() //�������� Ʋ���°� �˻��ϴ� �ڷ�ƾ
    {
        wrongAnswerChecking = true;
        float currentTime = 0;
        int wrong = 0;
        int wrongtry = wrongTry; //���� Ʋ�� ����
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
                    enemySpawnManager.SpawnEliteEnemy();
                    wrongAlert = 0;
                    break;
                }
            }
        }
        wrongAnswerChecking = false;
    }

    /// <summary>
    /// ������ ���� �޾ƿ���
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
                break;
            case "E":
                currentStatus = CurrentStatus.LEARNING;
                wj_conn.Learning_GetQuestion();
                break;
        }
    }

    /// <summary>
    ///  n ��° �н� ���� �޾ƿ���
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
    /// �޾ƿ� �����͸� ������ ������ ǥ��
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);
        
        string correctAnswer;
        string[] wrongAnswers;

        int ran = UnityEngine.Random.Range(0, 5);

        if (int.TryParse(qstCransr, out int result) == false) //������ ������ �ƴϸ� ������ ��ư���·� ������
        {
            ran = 1;
        }
        textCn = ProblemDescriptionLan(textCn);
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

            int ansrIndex = UnityEngine.Random.Range(0, ansrCount);

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

    //��ǲ�ʵ�� �� �־��� ��
    public void WriteAnswer(string answer)
    {
        bool isCorrect;
        string ansrCwYn = "N";
        tryCount += 1;
        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                try
                {
                    isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }
                catch
                {
                    wj_conn.Learning_GetQuestion();
                }
                finally
                {
                    isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }
                ansrCwYn = isCorrect ? "Y" : "N";
                if (isCorrect)
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(answer, ansrCwYn, (int)(questionSolveTime * 1000));
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
                isCorrect = true;
                try
                {
                    isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }
                catch
                {
                    wj_conn.Learning_GetQuestion();
                }
                finally
                {
                    isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }

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

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
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
    /// ���� ������ �¾Ҵ� �� üũ
    /// </summary>
    public void SelectAnswer(int _idx)
    {
        bool isCorrect;
        string ansrCwYn = "N";
        tryCount += 1;
        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                try
                {
                    isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                }
                catch (Exception ex)
                {
                    wj_conn.Learning_GetQuestion();
                }
                finally
                {
                    isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                }
                ansrCwYn = isCorrect ? "Y" : "N";
                if(ansrCwYn == "Y")
                {
                    correctCount += 1;
                    correctAudio.Play();
                    moneyManager.GetMoney(wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));
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
                try
                {
                    isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }
                catch(Exception ex)
                {
                    wj_conn.Learning_GetQuestion();
                }
                finally
                {
                    isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                }

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

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
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


    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        wj_conn.FirstRun_Diagnosis(a);
    }
    public void ButtonEvent_GetLearning()
    {
        wj_conn.Learning_GetQuestion();
    }
    public string ProblemDescriptionLan(string s)
    {
        string problem = "";
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            problem = s;
        }
        else
        {
            if (s == "다음 가분수를 자연수나 대분수로 바꾸어 보세요.")
            {
                problem = "Convert the following improper fractions into whole numbers or mixed numbers.";
            }
            else if (s == "두 수의 최대공약수를 구해 보세요.")
            {
                problem = "Find the greatest common divisor of two numbers.";
            }
            else if (s == "다음 덧셈을 하세요.")
            {
                problem = "Do the following addition.";
            }
            else if(s == "다음 방정식을 풀어 보세요.")
            {
                problem = "Solve the following equation.";
            }
            else if(s == "다음 뺄셈을 하세요.")
            {
                problem = "Do the following subtraction.";
            }
            else if(s == "다음 곱셈을 하세요.")
            {
                problem = "Do the following multiplication.";
            }
        }
        return problem;

    }
    #endregion
}
