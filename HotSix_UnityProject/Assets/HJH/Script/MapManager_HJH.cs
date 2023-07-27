using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager_HJH : MonoBehaviour
{
    public Sprite[] stageBGs;
    public int stage = 0;

    public SpriteRenderer bgSprite;
    public GameObject playerTower;
    public GameObject enemyTower;
    public GameObject laneManager;
    public CameraMove_HJH cameraMove;

    public Vector3 GetBGSize(GameObject bG)
    {
        Vector2 bGSpriteSize = bG.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localbGSize = bGSpriteSize / bG.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bG.transform.lossyScale.x;
        worldbGSize.y *= bG.transform.lossyScale.y;
        return worldbGSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        stage = GameManager.instance.stage;
        if(stage < 1)
        {
            stage = 1;
        }
        bgSprite.sprite = stageBGs[stage-1];
        Vector3 bgSize = GetBGSize(gameObject);
        playerTower.transform.position = new Vector3(-(bgSize.x / 2 - 2.5f), playerTower.transform.position.y, playerTower.transform.position.z);
        enemyTower.transform.position = new Vector3((bgSize.x/2 - 2.5f),enemyTower.transform.position.y,enemyTower.transform.position.z);
        Vector3 laneSize = GetBGSize(laneManager.transform.GetChild(0).gameObject);
        float laneX = laneSize.x/2;
        laneManager.transform.localScale = new Vector3((bgSize.x / 2 - 2.5f) / laneX, laneManager.transform.localScale.y, laneManager.transform.localScale.z);
        cameraMove.FirstSetting();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
