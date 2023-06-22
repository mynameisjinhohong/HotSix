using UnityEngine;

public class MathUI_HJH : MonoBehaviour
{
    bool hold = false;
    Vector2 startPos;
    public GameObject images;
    State state = State.ProblemOff;
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

        if (Input.GetMouseButtonDown(0))
        {
            hold = true;
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 movePos = (Vector2)Input.mousePosition - startPos;
            if (movePos.y > 0 && state == State.ProblemOff)
            {
                state = State.ProblemOn;
                images.SetActive(true);
            }
            else if (movePos.y < 0 && state == State.ProblemOn)
            {
                state = State.ProblemOff;
                images.SetActive(false);
            }
            hold = false;
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
                if (movePos.y > 0 && state == State.ProblemOff)
                {
                    state = State.ProblemOn;
                    images.SetActive(true);
                }
                else if (movePos.y < 0 && state == State.ProblemOn)
                {
                    state = State.ProblemOff;
                    images.SetActive(false);
                }
                hold = false;
            }
        }
#endif



    }
}
