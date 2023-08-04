using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearRewardUIManager_HJH : MonoBehaviour
{
    public GameObject rewardImage;
    public Transform rewardImageParent;
    public StageButtonManager stageButtonManager;
    public TMP_Text[] startAmoutTexts;
    public List<GameObject> instantiatedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(GameManager.instance.currentStage == null) return;
        int stage = (int)GameManager.instance.currentStage;
        RewardData_HJH reward = GameManager.instance.rewardData[stage];
        if (reward.random)
        {
            GameObject rI = Instantiate(rewardImage, rewardImageParent);
            instantiatedObject.Add(rI);  
            rI.GetComponent<Image>().sprite = GameManager.instance.unitImage[0];
        }
        else
        {
            GameObject rI = Instantiate(rewardImage, rewardImageParent);
            instantiatedObject.Add(rI);
            rI.GetComponent<Image>().sprite = GameManager.instance.unitImage[reward.confirmedUnitIdx];
            GameObject rI2 = Instantiate(rewardImage, rewardImageParent);
            instantiatedObject.Add(rI2);
            rI2.GetComponent<Image>().sprite = GameManager.instance.unitImage[0];
        }
        for(int i =0; i<3; i++)
        {
            startAmoutTexts[i].text = "X" + reward.startCardAmount[i];
        }

    }

    public void DestoryObject()
    {
        int a = instantiatedObject.Count;
        for(int i =0; i < a; i++)
        {
            Destroy(instantiatedObject[0].gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
