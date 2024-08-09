using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WeaponData : MonoBehaviour
{
    public string Name;
    public float Attack;
    public float Rate;

    #region Item Json ó��
    public WeaponData weaponData;
    #endregion
    void SaveWeaponDataToString()
    {
        string jsonData = JsonUtility.ToJson(weaponData, true);
        string folderPath = Path.Combine(Application.dataPath, "Item");
        string path = Path.Combine(folderPath, "weaponData.json");

        // JSON �����͸� ���Ϸ� ����
        File.WriteAllText(path, jsonData);

        Debug.Log("Weapon data saved to " + path);
    }
}
