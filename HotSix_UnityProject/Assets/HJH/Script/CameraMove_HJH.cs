using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove_HJH : MonoBehaviour
{
    public float cameraMoverSpeed;
    public float startPoint;
    public float endPoint;
    Vector2 click;

    public bool isActive = true;

    public GameObject background;
    public Vector3 backgroundSize;

    void Start()
    {
        Vector2 bgSpriteSize = background.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localBGSize = bgSpriteSize / background.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        backgroundSize = localBGSize;
        backgroundSize.x *= background.transform.lossyScale.x;
        backgroundSize.y *= background.transform.lossyScale.y;

        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        startPoint = background.transform.position.x - backgroundSize.x / 2.0f + cameraWidth;
        endPoint = background.transform.position.x + backgroundSize.x / 2.0f - cameraWidth;

        transform.position = new Vector3(startPoint, 0, -10);
    }
    public void FirstSetting()
    {
        Vector2 bgSpriteSize = background.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localBGSize = bgSpriteSize / background.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        backgroundSize = localBGSize;
        backgroundSize.x *= background.transform.lossyScale.x;
        backgroundSize.y *= background.transform.lossyScale.y;
        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        startPoint = background.transform.position.x - backgroundSize.x / 2.0f + cameraWidth;
        endPoint = background.transform.position.x + backgroundSize.x / 2.0f - cameraWidth;

        transform.position = new Vector3(startPoint, 0, -10);
    }
    
    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.GamePlay && isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
            #if UNITY_EDITOR
                click = Input.mousePosition;
            #else
                click = Input.GetTouch(0).position;      
            #endif
            }
            if (Input.GetMouseButton(0))
            {
            #if UNITY_EDITOR
                Vector2 stageMove = (Vector2)Input.mousePosition - click;
            #else
                Vector2 stageMove = (Vector2)Input.GetTouch(0).position - click;      
            #endif
                gameObject.transform.position -= new Vector3(stageMove.x / 500 * cameraMoverSpeed, 0, -10);
                gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, startPoint, endPoint), 0, 0);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, -10);
                click = Input.mousePosition;
            }
        }

    }
}
