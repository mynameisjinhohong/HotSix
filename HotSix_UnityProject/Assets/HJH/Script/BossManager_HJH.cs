using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager_HJH : MonoBehaviour
{
    public bool boss = false;
    public float camMoveSpeed = 1.0f;
    public GameObject bossObjcet;
    public GameObject enemyTower;
    public CameraMove_HJH cameraMove;
    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        if(GameManager.instance.stage == 12)
        {
            boss = true;
        }
        if (boss)
        {
            enemyTower.SetActive(false);
            cameraMove.enabled = false;
            bossObjcet.SetActive(true);

            StartCoroutine(moveCam());
        }
    }

    IEnumerator moveCam()
    {
        while (true)
        {
            bossObjcet.transform.position = new Vector3(enemyTower.transform.position.x,0,enemyTower.transform.position.z);
            mainCam.transform.position += Vector3.right * Time.deltaTime * camMoveSpeed;
            if(mainCam.transform.position.x >= cameraMove.endPoint)
            {
                mainCam.transform.position = new Vector3(cameraMove.endPoint, mainCam.transform.position.y, mainCam.transform.position.z);
                break;
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
