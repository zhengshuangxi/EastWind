using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 文件助手类
/// </summary>
public class FileHelper {

    private static string filePath =
#if UNITY_ANDROID && !UNITY_EDITOR
                     "jar:file://" + Application.dataPath + "!/assets/" + "PoliceOffice.csv";  
#elif UNITY_IPHONE && !UNITY_EDITOR
                      Application.dataPath + name;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                     "file://" + Application.streamingAssetsPath + "/" + "PoliceOffice.csv";
#endif

//    void Awake()
//    {
//        filePath =
//#if UNITY_ANDROID && !UNITY_EDITOR
//                     "jar:file://" + Application.dataPath + "!/assets/" + "PoliceOffice.csv";  
//#elif UNITY_IPHONE && !UNITY_EDITOR
//                      Application.dataPath + name;  
//#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
//                     "file://" + Application.streamingAssetsPath + "/" + "PoliceOffice.csv";
//#endif
//    }

    public static IEnumerator Load(Action<string> callback = null, Action Oncomplete = null)
    {
        WWW www = new WWW(filePath);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }
        string _result = Encoding.UTF8.GetString(www.bytes);
//#if UNITY_ANDROID && !UNITY_EDITOR
//        _result = Encoding.Default.GetString(www.bytes);
//#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
//        _result = Encoding.Default.GetString(www.bytes);
//#endif
        string[] content = _result.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < content.Length; i++)
        {
            callback(content[i]);
        }

        if (Oncomplete != null)
            Oncomplete();
    }
}
