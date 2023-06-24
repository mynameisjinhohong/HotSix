using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUI_HJH : MonoBehaviour
{
    public float startPoint;
    public float endPoint;
    public Image stageImage;
    Vector2 click;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 stageMove = (Vector2)Input.mousePosition - click;
            if(stageImage.rectTransform.position.x >= endPoint && stageImage.rectTransform.position.x <=startPoint)
            {
                stageImage.rectTransform.position += new Vector3(stageMove.x,0,0);
                stageImage.rectTransform.position = new Vector3(Mathf.Clamp(stageImage.rectTransform.position.x, endPoint, startPoint), 0, 0); //시작과 끝 제한
            }
            stageImage.rectTransform.position = new Vector3(stageImage.rectTransform.position.x, 0,0);
            click = Input.mousePosition;

        }
        if(Input.GetMouseButtonUp(0))
        {

        }
    }

    public void MoveStage(int stage)
    {
        GameManager.instance.stage = stage;
        SceneManager.LoadScene("GameScene");
    }
}
