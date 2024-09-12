using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavesController
{
    public static void Save<T>(string file, T obj)
    {
        string jsonObj = JsonUtility.ToJson(obj, true);

        PlayerPrefs.SetString(file, XorCipher.Encrypt(jsonObj));
    }

    public static T GetData<T>(string file) where T: new()
    {
        if (PlayerPrefs.HasKey(file))
        {
            string jsonObjString = PlayerPrefs.GetString(file);
            return JsonUtility.FromJson<T>(XorCipher.Decrypt(jsonObjString));
        }
        else
        {
            return new T();
        }
    }

    
}

