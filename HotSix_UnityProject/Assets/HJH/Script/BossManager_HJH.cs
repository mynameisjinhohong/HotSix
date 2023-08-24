using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoreanTyper;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class BossManager_HJH : MonoBehaviour
{
    public bool boss = false;
    public float camMoveSpeed = 1.0f;
    public float camBackSpeed = 5f;
    public float bossHpSpeed = 1f;
    public float bossGrowSpeed = 1f;
    public float bossFullScale = 5f;
    //public float bossTurnSpeed = 10f;
    //public float bossColorSpeed = 1f;
    public GameObject bossObjcet;
    public GameObject enemyTower;
    public GameObject bossShadow;
    public TMP_Text bossText;
    public Slider bossSlider;

    [Header("보스전 때 껐다 켜줘야 할 것들")]
    public TowerHPManager_HJH hpManager;
    public CameraMove_HJH cameraMove;
    public MoneyManager_HJH moneyManager;
    public EnemySpawnManager_MJW enemySpawn;
    public MathUI_HJH mathUI;
    public Menu_HJH menu;


    Camera mainCam;
    
    private void Awake()
    {
        hpManager.boss = true;
        bossObjcet.SetActive(false);
        mainCam = Camera.main;
        bossText.text = "";
        if (GameManager.instance.stage == 12)
        {
            boss = true;
        }
        if (boss)
        {
            enemyTower.SetActive(false);
            OnOff(false);
            bossObjcet.SetActive(true);

            StartCoroutine(MoveCam());
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnOff(bool on)
    {
        if (on)
        {
            hpManager.enabled = true;
            cameraMove.enabled = true;
            moneyManager.enabled = true;
            enemySpawn.enabled = true;
            mathUI.enabled = true;
            menu.enabled = true;
        }
        else
        {
            hpManager.enabled = false;
            cameraMove.enabled = false;
            moneyManager.enabled = false;
            enemySpawn.enabled = false;
            mathUI.enabled=false;
            menu.enabled = false;
        }
    }

    IEnumerator MoveCam()
    {
        while (true)
        {
            bossObjcet.transform.position = new Vector3(enemyTower.transform.position.x,bossObjcet.transform.position.y,enemyTower.transform.position.z);
            mainCam.transform.position += Vector3.right * Time.deltaTime * camMoveSpeed;
            if(mainCam.transform.position.x >= cameraMove.endPoint)
            {
                mainCam.transform.position = new Vector3(cameraMove.endPoint, mainCam.transform.position.y, mainCam.transform.position.z);
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    StartCoroutine(BossTalk("You still don't know the true power of the negative kingdom.", 0));
                }
                else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                {
                    StartCoroutine(BossTalk("너희는 아직 음수 왕국의 진정한 힘을 모른다.", 0));
                }
                break;
            }
            yield return null;
        }
    }

    IEnumerator BossTalk(string text, int su) // 무슨 말을 할것인지, 몇번째 대사인지
    {
        int typeLength = text.GetTypingLength();
        for (int i = 0; i < typeLength; i++)
        {
            bossText.text = text.Typing(i);
            yield return new WaitForSeconds(0.03f);
        }
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bossText.text = "";
                if (su == 0)
                {
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                    {
                        StartCoroutine(BossTalk("I'll show you what that power is from now on!", su + 1));
                        break;
                    }
                    else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                    {
                        StartCoroutine(BossTalk("지금부터 그 힘이 무엇인지 보여주마!", su + 1));
                        break;
                    }
                }
                else if (su == 1)
                {
                    bossText.text = "";
                    StartCoroutine(BossHpProgress());
                    StartCoroutine(BossGrow());
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator BossHpProgress()
    {
        while (true)
        {

            if(hpManager.enemyHPSlider.value >= 1)
            {
                bossSlider.value += 0.1f * bossHpSpeed;
                if(bossSlider.value >= 1)
                {
                    break;
                }
            }
            else
            {
                hpManager.enemyHPSlider.value += 0.1f * bossHpSpeed;
                if(hpManager.enemyHPSlider.value >= 1)
                {
                    bossSlider.gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator BossGrow()
    {
        GameManager.instance.Vibrate();
        StartCoroutine(Shake(amount,duration));
        while (true)
        {
            bossObjcet.transform.localScale += new Vector3(0.1f, 0.1f, 0) * bossGrowSpeed;
            if(bossObjcet.transform.localScale.x >= bossFullScale)
            {
                StartCoroutine(ReturnGame());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    #region  주석들
    //IEnumerator BossShadow()
    //{
    //    while(true)
    //    {
    //        Color a = bossShadow.GetComponent<SpriteRenderer>().color;
    //        a.a -= bossColorSpeed * Time.deltaTime;
    //        bossShadow.GetComponent<SpriteRenderer>().color = a;
    //        if(a.a < 0)
    //        {
    //            break;
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator BossTurn()
    //{
    //    while (true)
    //    {
    //        bossObjcet.transform.Rotate(Vector3.down , -1f*Time.deltaTime * bossTurnSpeed);
    //        if(bossObjcet.transform.rotation.y <= 0)
    //        {
    //            StartCoroutine(ReturnGame());
    //            break;
    //        }
    //        yield return null;
    //    }
    //}
    #endregion  

    #region 카메라 쉐이크
    [SerializeField]
    public float duration;      //진행 속도
    public float amount;      //움직임 범위

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        Vector3 originPos = mainCam.transform.position;
        while (timer <= _duration)
        {
            Vector3 movePos = (Vector3)Random.insideUnitCircle * _amount + originPos;
            if(movePos.x >= cameraMove.endPoint)
            {
                movePos.x = cameraMove.endPoint;
            }
            mainCam.transform.position = movePos;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;

    }
    #endregion


    IEnumerator ReturnGame()
    {
        while (true)
        {
            mainCam.transform.position += Vector3.left * Time.deltaTime * camBackSpeed;
            if (mainCam.transform.position.x <= cameraMove.startPoint)
            {
                mainCam.transform.position = new Vector3(cameraMove.startPoint, mainCam.transform.position.y, mainCam.transform.position.z);
                OnOff(true);
                break;
            }
            yield return null;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
