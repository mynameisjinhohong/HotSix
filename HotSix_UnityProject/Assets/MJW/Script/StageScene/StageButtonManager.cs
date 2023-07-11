using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButtonManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] buttons;
    public GameObject StagePopUp;
    public int? selectedIndex = null;

    public AudioSource audio;

    private RaycastHit[] hits;

    public void ResetButton(){
        audio.Play();
        selectedIndex = null;
    }
    public void FirstResetButton()
    {
        selectedIndex = null;
    }
    public int? CheckButton(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(ray);
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider.tag == "Button"){
                for(int j = 0; j < buttons.Length; ++j){
                    if(System.Object.ReferenceEquals(buttons[j], hit.collider.gameObject)){
                        return j + 1;
                    }
                }
            }
        }
        return null;
    }

    public void MoveStage(){
        audio.Play();
        gameManager.stage = (int)selectedIndex;
        Invoke("MoveScene", 0.1f);
    }
    public void MoveScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        int count = transform.childCount;
        buttons = new GameObject[count];
        for(int i = 0; i < count; ++i){
            buttons[i] = transform.GetChild(i).gameObject;
        }

        StagePopUp.SetActive(false);
        FirstResetButton();
    }

    void Update()
    {
        if(selectedIndex == null && Input.GetMouseButtonDown(0)){
            selectedIndex = CheckButton();
            if(selectedIndex != null){
                audio.Play();
                StagePopUp.SetActive(true);
            }
        }
    }
}
