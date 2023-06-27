using UnityEngine;

public class CameraMove_HJH : MonoBehaviour
{
    public float cameraMoverSpeed;
    public float startPoint;
    public float endPoint;
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
            gameObject.transform.position += new Vector3(stageMove.x/500 * cameraMoverSpeed, 0, -10);
            gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, startPoint, endPoint), 0, 0); //시작과 끝 제한
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, -10);
            click = Input.mousePosition;
        }
    }
}
