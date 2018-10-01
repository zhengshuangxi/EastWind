using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Util
{
    public class Path
    {
        public static string GetStreamAssetsPath(string name)
        {
            string filePath =
#if UNITY_ANDROID && !UNITY_EDITOR
                     "jar:file://" + Application.dataPath + "!/assets/" + name;  
#elif UNITY_IPHONE && !UNITY_EDITOR
                      Application.dataPath + name;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                     "file://" + Application.streamingAssetsPath + "/" + name;
#else
                     string.Empty;  
#endif
            return filePath;
        }

        public static bool ExistInStreamingAssets(string name)
        {
            string path = GetStreamAssetsPath(name);
            if (File.Exists(path))
                return true;
            else
                return false;
        }
    }
}
