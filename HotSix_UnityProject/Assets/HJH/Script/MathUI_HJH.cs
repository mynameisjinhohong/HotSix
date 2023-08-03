using System;
using UnityEngine;
using UnityEngine.UI;

public class MathUI_HJH : MonoBehaviour
{
    bool hold = false;
    Vector2 startPos;
    public GameObject images;
    public Button OnButton;
    public Button OffButton;
    State state = State.ProblemOff;
    public AudioSource audioSource;
    enum State
    {
        ProblemOn,
        ProblemOff,
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if(GameManager.instance.gameState == GameManager.GameState.GamePlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hold = true;
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 movePos = (Vector2)Input.mousePosition - startPos;
                if (movePos.y > 100 && Math.Abs(movePos.y) > Math.Abs(movePos.x) && state == State.ProblemOn)
                {
                    //Debug.Log("y : " + movePos.y + "\nx : " + movePos.x);
                    ProblemOFF();
                }
                else if (movePos.y<-100 && Math.Abs(movePos.y) > Math.Abs(movePos.x) && state == State.ProblemOff)
                {
                    //Debug.Log("y : " + movePos.y + "\nx : " + movePos.x);
                    ProblemON();

                }
                hold = false;
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
if (Input.GetMouseButtonDown(0))
            {
                hold = true;
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 movePos = (Vector2)Input.mousePosition - startPos;
                if (movePos.y > 100 && Math.Abs(movePos.y) > Math.Abs(movePos.x) && state == State.ProblemOn)
                {
                    ProblemOFF();
                }
                else if (movePos.y<-100 && Math.Abs(movePos.y) > Math.Abs(movePos.x) && state == State.ProblemOff)
                {
                    ProblemON();

                }
                hold = false;
            }
        }
#endif



    }

    public void ProblemON()
    {
        if (state == State.ProblemOff)
        {
            audioSource.Play();
            OnButton.gameObject.SetActive(false);
            OffButton.gameObject.SetActive(true);
            state = State.ProblemOn;
            images.SetActive(true);
        }
    }
    public void ProblemOFF()
    {
        if (state == State.ProblemOn)
        {
            audioSource.Play();
            OnButton.gameObject.SetActive(true);
            OffButton.gameObject.SetActive(false);
            state = State.ProblemOff;
            images.SetActive(false);
        }
    }
}
