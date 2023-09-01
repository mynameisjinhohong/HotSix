using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager_HJH : MonoBehaviour
{
    public float minWait = 0.5f;
    static string nextScene = "StartScene";
    // Start is called before the first frame update
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");

    }
    private void Start()
    {
        StartCoroutine(LoadSceneCo());
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
                Debug.Log("!!!");
                break;
            }

        }
    }
}
