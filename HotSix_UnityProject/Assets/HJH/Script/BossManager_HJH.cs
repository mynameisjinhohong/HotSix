using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoreanTyper;
using TMPro;
using UnityEngine.Localization.Settings;

public class BossManager_HJH : MonoBehaviour
{
    public bool boss = false;
    public float camMoveSpeed = 1.0f;
    public float camBackSpeed = 5f;
    public float bossTurnSpeed = 10f;
    public float bossColorSpeed = 1f;
    public GameObject bossObjcet;
    public GameObject enemyTower;
    public GameObject bossShadow;
    public TMP_Text bossText;
    [Header("보스전 때 껐다 켜줘야 할 것들")]
    public CameraMove_HJH cameraMove;
    public MoneyManager_HJH moneyManager;
    public EnemySpawnManager_MJW enemySpawn;
    public MathUI_HJH mathUI;
    public Menu_HJH menu;


    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        bossObjcet.SetActive(false);
        mainCam = Camera.main;
        bossText.text = "";
        if(GameManager.instance.stage == 12)
        {
            boss = true;
        }
        if (boss)
        {
            enemyTower.SetActive(false);
            OnOff(false );
            bossObjcet.SetActive(true);

            StartCoroutine(MoveCam());
        }
    }

    void OnOff(bool on)
    {
        if (on)
        {
            cameraMove.enabled = true;
            moneyManager.enabled = true;
            enemySpawn.enabled = true;
            mathUI.enabled = true;
            menu.enabled = true;
        }
        else
        {
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
            bossObjcet.transform.position = new Vector3(enemyTower.transform.position.x,0,enemyTower.transform.position.z);
            mainCam.transform.position += Vector3.right * Time.deltaTime * camMoveSpeed;
            if(mainCam.transform.position.x >= cameraMove.endPoint)
            {
                mainCam.transform.position = new Vector3(cameraMove.endPoint, mainCam.transform.position.y, mainCam.transform.position.z);
                StartCoroutine(BossShadow());
                StartCoroutine(BossTurn());
                break;
            }
            yield return null;
        }
    }

    IEnumerator BossShadow()
    {
        while(true)
        {
            Color a = bossShadow.GetComponent<SpriteRenderer>().color;
            a.a -= bossColorSpeed * Time.deltaTime;
            bossShadow.GetComponent<SpriteRenderer>().color = a;
            if(a.a < 0)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator BossTurn()
    {
        while (true)
        {
            bossObjcet.transform.Rotate(Vector3.down , -1f*Time.deltaTime * bossTurnSpeed);
            if(bossObjcet.transform.rotation.y <= 0)
            {
                if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {

                    StartCoroutine(BossTalk("You've come this far..",0));
                }
                else if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                {
                    StartCoroutine(BossTalk("잘도 여기까지 왔군..", 0));
                }
                break;
            }
            yield return null;
        }
    }

    IEnumerator BossTalk(string text, int su) // 무슨 말을 할것인지, 몇번째 대사인지
    {
        int typeLength = text.GetTypingLength();
        for(int i =0; i < typeLength; i++)
        {
            bossText.text = text.Typing(i);
            yield return new WaitForSeconds(0.03f);
        }
        while (true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                bossText.text = "";
                if (su == 0)
                {
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                    {
                       StartCoroutine(BossTalk("But your journey ends here!", su + 1));
                        break;
                    }
                    else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                    {
                        StartCoroutine(BossTalk("하지만 너의 여정은 여기까지다!", su + 1));
                        break;
                    }
                }
                else if(su == 1)
                {
                    bossText.gameObject.SetActive(false);
                    StartCoroutine(ReturnGame());
                    break;
                }
            }
            yield return null;  
        }
    }

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
