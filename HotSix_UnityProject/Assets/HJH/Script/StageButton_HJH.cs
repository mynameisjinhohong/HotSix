using System.Collections;
using System.Collections.Generic;
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
        for(int i = 0; i < 3; i++)
        {
            if(GameManager.instance.userData.stageStar[stage].stageStar[i] == true)
            {
                stageClear++;
            }
        }
        for(int i = 0; i < 3; i++) 
        {
            SpriteRenderer sprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if(i < stageClear)
            {
                sprite.sprite = GameManager.instance.starImage[1];
            }
            else
            {
                sprite.sprite = GameManager.instance.starImage[0];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
