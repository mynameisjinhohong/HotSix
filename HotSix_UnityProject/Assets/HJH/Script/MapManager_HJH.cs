using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager_HJH : MonoBehaviour
{


    public int stage = 0;

    public GameObject[] lane1Collider;
    public GameObject[] lane2Collider;

    public SpriteRenderer bgSprite;
    public GameObject playerTower;
    public GameObject enemyTower;
    public GameObject lane;
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
        bgSprite.sprite = GameManager.instance.mapElements[stage].stageBG;
        Vector3 bgSize = GetBGSize(gameObject);
        playerTower.transform.position = new Vector3(-(bgSize.x / 2 - 2.25f), playerTower.transform.position.y, playerTower.transform.position.z);
        enemyTower.transform.position = new Vector3((bgSize.x/2 - 2.25f),enemyTower.transform.position.y,enemyTower.transform.position.z);
        GameObject laneParent;
        if (GameManager.instance.mapElements[stage].lineCount == 1)
        {
            laneParent= lane.transform.GetChild(0).gameObject;
            lane.transform.GetChild(0).gameObject.SetActive(true);
            lane.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            laneParent = lane.transform.GetChild(1).gameObject;
            lane.transform.GetChild(0).gameObject.SetActive(false);
            lane.transform.GetChild(1).gameObject.SetActive(true);
        }
        Vector3 laneSize = GetBGSize(laneParent.transform.GetChild(0).gameObject);
        float laneX = laneSize.x/2;
        laneParent.transform.localScale = new Vector3((bgSize.x / 2 - 2.5f) / laneX, lane.transform.localScale.y, lane.transform.localScale.z);
        lane1Collider[0].transform.position = new Vector3(-(bgSize.x / 2 - 4f), lane1Collider[0].transform.position.y, lane1Collider[0].transform.position.z);
        lane1Collider[1].transform.position = new Vector3((bgSize.x / 2 - 4f), lane1Collider[1].transform.position.y, lane1Collider[1].transform.position.z);
        lane2Collider[0].transform.position = new Vector3(-(bgSize.x / 2 - 3.5f), lane2Collider[0].transform.position.y, lane2Collider[0].transform.position.z);
        lane2Collider[1].transform.position = new Vector3((bgSize.x / 2 - 3.5f), lane2Collider[1].transform.position.y, lane2Collider[1].transform.position.z);
        cameraMove.FirstSetting();
        
    }

    public void MovePlayerTower()
    {
        playerTower.transform.position += new Vector3(-0.5f, 0, 0);
    }

    public void MoveEnemyTower()
    {
        enemyTower.transform.position += new Vector3(0.5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
