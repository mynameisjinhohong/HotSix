using UnityEngine;

public class StageButton_HJH : MonoBehaviour
{
    public int stage;
    public Sprite nowButtonImage;
    public Sprite clearButtonIamge;
    // Start is called before the first frame update
    void Start()
    {
        int stageClear = 0;
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.instance.userData.stageStar[stage].stageStar[i] == true)
            {
                stageClear++;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        switch (stageClear)
        {
            case 1:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 3:
                transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
