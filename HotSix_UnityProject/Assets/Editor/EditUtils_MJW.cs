using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditUtils_MJW
{
    [MenuItem("Utils/Clear Data")]
    static public void ClearData(){
        string filePath = Application.persistentDataPath;
        System.IO.File.Delete(filePath + "/UserData.txt");

        PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
    }
}
