using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCharacterEditManager_HJH : MonoBehaviour
{
    public GameObject UnitImageButton;
    public Transform instantiateTransform;
    public ProfileManager_HJH[] profiles;
    public RectTransform scrollViewContent;
    int unitCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        UserInfo_MJW userInfo = GameManager.instance.userInfo;
        for(int i = 0; i<userInfo.userUnitInfo.Count; i++)
        {
            if(i == 0)
            {
                continue;
            }
            if (userInfo.userUnitInfo[i].level > 0)
            {
                unitCount++;
                GameObject button = Instantiate(UnitImageButton, instantiateTransform);
                button.GetComponent<Image>().sprite = GameManager.instance.unitImages.playerUnitImages[userInfo.userUnitInfo[i].id].iconImage;
                int id = userInfo.userUnitInfo[i].id;
                button.GetComponent<Button>().onClick.AddListener(() => ChangeUnitImage(id));
            }
        }
        scrollViewContent.sizeDelta = new Vector2((unitCount * 350),scrollViewContent.sizeDelta.y);

    }

    public void ChangeUnitImage(int id)
    {
        GameManager.instance.userData.porfileImg = id;
        for(int i = 0; i<profiles.Length; i++)
        {
            profiles[i].ProfileEditApply();
        }
        gameObject.SetActive(false);
    }
}
