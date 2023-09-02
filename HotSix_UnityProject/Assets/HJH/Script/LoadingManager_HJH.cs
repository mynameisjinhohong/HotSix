using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager_HJH : MonoBehaviour
{
    public TMP_Text loadingText;
    public float minWait = 0.5f;
    public MeshRenderer bgMat;
    public float bgScrollSpeed = 1.0f;
    static string nextScene = "StartScene";
    public float textSpeed = 1.0f;
    // Start is called before the first frame update
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");

    }
    private void Start()
    {
        StartCoroutine(LoadSceneCo());
        StartCoroutine(LoadingText());
        StartCoroutine(BgScroll());
    }

    // Update is called once per frame
    IEnumerator LoadSceneCo()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        float currentTime = 0;
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            currentTime += Time.deltaTime;
            yield return null;
            if(currentTime > minWait)
            {
                async.allowSceneActivation = true;
                break;
            }

        }
    }

    IEnumerator BgScroll()
    {
        Material mat = bgMat.material;
        while (true)
        {
            mat.mainTextureOffset += Vector2.right * bgScrollSpeed * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator LoadingText()
    {
        loadingText.text = "Loading";
        string text1 = "Loading";
        string text2 = "Loading.";
        string text3 = "Loading..";
        string text4 = "Loading...";
        int idx = 0;
        while (true)
        {
            idx++;
            yield return new WaitForSeconds(textSpeed);
            switch (idx)
            {
                case 0:
                    loadingText.text = text1;
                    break;
                case 1: 
                    loadingText.text = text2;
                    break;
                case 2:
                    loadingText.text = text3;
                    break;
                case 3:
                    loadingText.text = text4;
                    idx = 0;
                    break;
            }
        }
    }
}
