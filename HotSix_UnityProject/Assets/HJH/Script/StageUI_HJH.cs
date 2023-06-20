using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


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
                Debug.Log("?");
                stageImage.rectTransform.position += new Vector3(stageMove.x,0,0);
                if(stageImage.rectTransform.position.x <endPoint)
                {
                    stageImage.rectTransform.position = new Vector3(endPoint,0,0);
                    Debug.Log("??");
                }
                if(stageImage.rectTransform.position.x > startPoint)
                {
                    stageImage.rectTransform.position = new Vector3(startPoint,0,0);
                    Debug.Log("???");
                }
            }
            stageImage.rectTransform.position = new Vector3(stageImage.rectTransform.position.x, 0,0);
            Debug.Log(stageImage.rectTransform.position.y);
            click = Input.mousePosition;

        }
        if(Input.GetMouseButtonUp(0))
        {

        }
    }

}
