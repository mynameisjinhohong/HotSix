using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager_HJH : MonoBehaviour
{
    static string nextScene;
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
        //async.allowSceneActivation = false;
        while(async.isDone)
        {
            yield return null;
            
        }
    }
}
