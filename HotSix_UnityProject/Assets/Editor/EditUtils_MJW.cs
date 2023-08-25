using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditUtils_MJW
{
    [MenuItem("Utils/Clear PlayerPrefs")]
    static public void ClearPlayerPrefs(){
        PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
    }
}
