using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_HJH : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffetcSlider;

    public GameObject settingPopup;
    // Start is called before the first frame update
    void Start()
    {
        bgmSlider.onValueChanged.AddListener(BgmChange);
        soundEffetcSlider.onValueChanged.AddListener (SoundEffectChange);
        bgmSlider.value = GameManager.instance.BgmVolume;
        soundEffetcSlider.value = GameManager.instance.SoundEffectVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MenuButton()
    {
        if (settingPopup.activeInHierarchy)
        {
            Time.timeScale = 1f;
            settingPopup.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            settingPopup.SetActive(true);
        }
    }

    void BgmChange(float value)
    {
        GameManager.instance.BgmVolume = value;
    }
    void SoundEffectChange(float value)
    {
        GameManager.instance.SoundEffectVolume = value;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageScene");
    }
}
