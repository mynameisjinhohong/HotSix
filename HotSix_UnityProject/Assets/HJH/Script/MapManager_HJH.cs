using UnityEngine;


public class MapManager_HJH : MonoBehaviour
{
    [System.Serializable]
    public class mapSize
    {
        public Sprite[] mapsize;
        public GameObject lineParent;
    }
    [System.Serializable]
    public class lineCount
    {
        public mapSize[] linecount;
    }

    public lineCount[] sprites;

    public int stage = 0;
    //가장 밖에 있는 line 오브젝트
    public GameObject line;
    //실제 콜라이더가 있는 라인이 담긴 오브젝트 배열
    public GameObject[] lines;
    public SpriteRenderer bgSprite;
    public GameObject playerTower;
    public GameObject enemyTower;
    public CameraMove_HJH cameraMove;
    MapElement mapElement;
    public Vector3 GetBGSize(GameObject bG)
    {
        Vector2 bGSpriteSize = bG.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localbGSize = bGSpriteSize / bG.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bG.transform.lossyScale.x;
        worldbGSize.y *= bG.transform.lossyScale.y;
        return worldbGSize;
    }
    private void Awake()
    {
        stage = GameManager.instance.stage;
        mapElement = GameManager.instance.mapElements[stage];
        lines = new GameObject[GameManager.instance.mapElements[stage].lineCount];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = sprites[(int)mapElement.stageBG].linecount[mapElement.lineCount - 1].lineParent.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < 6; i++)
        {
            line.transform.GetChild(i).gameObject.SetActive(false);
            if (line.transform.GetChild(i).gameObject == sprites[(int)mapElement.stageBG].linecount[mapElement.lineCount - 1].lineParent)
            {
                sprites[(int)mapElement.stageBG].linecount[mapElement.lineCount - 1].lineParent.SetActive(true);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        bgSprite.sprite = sprites[(int)mapElement.stageBG].linecount[mapElement.lineCount - 1].mapsize[(int)mapElement.lineLength];
        Vector3 bgSize = GetBGSize(gameObject);
        playerTower.transform.position = new Vector3(-(bgSize.x / 2 - 1f), playerTower.transform.position.y, playerTower.transform.position.z);
        enemyTower.transform.position = new Vector3((bgSize.x / 2 - 1f), enemyTower.transform.position.y, enemyTower.transform.position.z);
        for(int i = 0; i< lines.Length; i++) 
        {
            BoxCollider lineCol = lines[i].gameObject.GetComponent<BoxCollider>();
            lineCol.size = new Vector3((bgSize.x - 6) / lines[i].transform.localScale.x,lineCol.size.y, lineCol.size.z);
            if (i == 0)
            {
                lines[i].transform.GetChild(0).position = new Vector3(-(bgSize.x / 2 - 4f), lines[i].transform.GetChild(0).position.y, lines[i].transform.GetChild(0).position.z);
                lines[i].transform.GetChild(1).position = new Vector3((bgSize.x / 2 - 4f), lines[i].transform.GetChild(1).position.y, lines[i].transform.GetChild(1).position.z);
            }
            else
            {
                lines[i].transform.GetChild(0).position = new Vector3(-(bgSize.x / 2 - 3.5f), lines[i].transform.GetChild(0).position.y, lines[i].transform.GetChild(0).position.z);
                lines[i].transform.GetChild(1).position = new Vector3((bgSize.x / 2 - 3.5f), lines[i].transform.GetChild(1).position.y, lines[i].transform.GetChild(1).position.z);
            }
        }
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
