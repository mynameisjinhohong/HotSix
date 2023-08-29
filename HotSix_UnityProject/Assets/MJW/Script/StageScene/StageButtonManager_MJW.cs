using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageButtonManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public GameObject[] buttons;
    public GameObject StagePopUp;

    public AudioSource audio;

    private RaycastHit[] hits;

    #endregion

    public TutorialManager_HJH tutorial;
    public bool tutoOK = false;
    #region Methods

    public void ResetButton()
    {
        audio.Play();
        gameManager.currentStage = null;
    }

    public int? CheckButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; ++i)
        {
            RaycastHit hit = hits[i];
            if (hit.collider == null) continue;
            if (hit.collider.CompareTag("Button"))
            {
                for (int j = 0; j < buttons.Length; ++j)
                {
                    if (System.Object.ReferenceEquals(buttons[j], hit.collider.gameObject))
                    {
                        return j + 1;
                    }
                }
            }
        }
        return null;
    }

    public void MoveStage()
    {
        audio.Play();
        gameManager.stage = (int)gameManager.currentStage;
        SceneManager.LoadScene("GameScene");
        // Invoke("MoveScene", 0.1f);
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    #endregion


    #region Monobehavior Callbacks

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        int count = transform.childCount;
        buttons = new GameObject[count];
        for (int i = 0; i < count; ++i)
        {
            buttons[i] = transform.GetChild(i).gameObject;
        }
        int clearStage = gameManager.userData.stageProgress;
        for (int i = 0; i < clearStage; ++i)
        {
            buttons[i].GetComponent<SpriteRenderer>().sprite = buttons[i].GetComponent<StageButton_HJH>().clearButtonIamge;
        }
        buttons[clearStage].GetComponent<SpriteRenderer>().sprite = buttons[clearStage].GetComponent<StageButton_HJH>().nowButtonImage;
        if(SceneManager.GetActiveScene().name != "TutorialScene")
        {
            StagePopUp.SetActive(gameManager.currentStage != null);
        }
    }

    void Update()
    {
        if (gameManager.currentStage == null && Input.GetMouseButtonDown(0) && GameManager.instance.gameState == GameManager.GameState.GamePlay && tutoOK == false)
        {
            gameManager.currentStage = CheckButton();
            if (gameManager.currentStage != null)
            {
                if (gameManager.currentStage <= GameManager.instance.userData.stageProgress + 1)
                {
                    audio.Play();
                    StagePopUp.SetActive(true);
                    if(tutorial != null)
                    {
                        tutorial.StageClick();
                    }
                }
                else
                {
                    gameManager.currentStage = null;
                }
            }
        }
    }

    #endregion
}
