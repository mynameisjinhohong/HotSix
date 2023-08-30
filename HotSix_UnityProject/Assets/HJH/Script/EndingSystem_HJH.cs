using KoreanTyper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingSystem_HJH : MonoBehaviour
{
    public GameObject[] scenes;
    public TMP_Text[] texts;
    public bool skip = false;
    public float textSpeed = 0.01f;
    public float fadeSpeed = 0.01f;
    public bool texting = false;
    public int idx = 0;
    public GameObject bg;
    public GameObject popUp;
    public bool popUpEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (texting)
        {
            if(Input.GetMouseButtonDown(0))
            {
                skip = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (idx)
                {
                    case 1:
                        StartCoroutine(FadeIn(scenes[1]));
                        StartCoroutine(TextAni(texts[1]));
                        break;
                    case 2:
                        StartCoroutine(TextAni(texts[2]));
                        break;
                    case 3:
                        StartCoroutine(FadeIn(scenes[2]));
                        StartCoroutine(TextAni(texts[3]));
                        break;
                    case 4:
                        StartCoroutine(TextAni(texts[4]));
                        break;
                    case 5:
                        StartCoroutine(FadeOut(scenes[2]));
                        break;
                }
            }
        }
        if (popUpEnd)
        {
            if(Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        GameManager.instance.bgm.clip = GameManager.instance.bgmSources[6];
        GameManager.instance.bgm.Play();
        StartCoroutine(TextAni(texts[0]));
    }
    IEnumerator TextAni(TMP_Text nowText)
    {
        texting = true;
        for(int i = 0;i<texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
        }
        nowText.gameObject.SetActive(true);
        float textScale = nowText.fontSize;
        nowText.enableAutoSizing = false;
        nowText.fontSize = textScale;
        string text = nowText.text;
        int typeLength = nowText.text.GetTypingLength();
        for (int i = 0; i < typeLength + 1; i++)
        {
            nowText.text = text.Typing(i);
            if (skip)
            {
                skip = false;
                nowText.text = text;
                break;
            }
            yield return new WaitForSeconds(textSpeed);
        }
        texting = false;
        idx++;
    }

    public void EndCutScene()
    {
        bg.SetActive(false);
        popUp.SetActive(true);
        for(int i =0; i< texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
        }
        for(int i =0; i<scenes.Length; i++)
        {
            scenes[i].gameObject.SetActive(false);
        }
        popUpEnd = true;
        GameManager.instance.bgm.clip = GameManager.instance.bgmSources[3];
        GameManager.instance.bgm.Play();
    }

    IEnumerator FadeIn(GameObject scene)
    {
        for(int i = 0; i<scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
        scene.SetActive(true);
        Image image = scene.GetComponent<Image>();
        float alpha = 0;
        while(alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(fadeSpeed);
            Color color = image.color;
            color.a = alpha;
            image.color = color;
            if (skip)
            {
                color.a = 1f;
                image.color = color;
                break;
            }
        }

    }
    IEnumerator FadeOut(GameObject scene)
    {
        Image image = scene.GetComponent<Image>();
        float alpha = 1;
        while (alpha > 0f)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(fadeSpeed);
            Color color = image.color;
            color.a = alpha;
            image.color = color;
            if (skip)
            {
                color.a = 0f;
                image.color = color;
                break;
            }
        }
        if(scene = scenes[2])
        {
            EndCutScene();
        }

    }
}
