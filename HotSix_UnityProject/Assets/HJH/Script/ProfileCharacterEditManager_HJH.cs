using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCharacterEditManager_HJH : MonoBehaviour
{
    public GameObject UnitImageButton;
    public ProfileManager_HJH[] profiles; 

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
                GameObject button = Instantiate(UnitImageButton, transform);
                button.GetComponent<Image>().sprite = GameManager.instance.unitImage[userInfo.userUnitInfo[i].id];
                int id = userInfo.userUnitInfo[i].id;
                button.GetComponent<Button>().onClick.AddListener(() => ChangeUnitImage(id));
            }
        }
    }

    public void ChangeUnitImage(int id)
    {
        GameManager.instance.userData.porfileImg = id;
        for(int i = 0; i<profiles.Length; i++)
        {
            profiles[i].ProfileEditApply();
        }
    }
}
