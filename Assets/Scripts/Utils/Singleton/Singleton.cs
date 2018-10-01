using UnityEngine;

namespace Util
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static object signal = new object();

        public static T GetInstance()
        {
            lock (signal)
            {
                if (null == instance)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return instance;
                    }

                    if (null == instance)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = string.Format("Singleton<{0}>", typeof(T));
                        DontDestroyOnLoad(singleton);
                    }
                }
                return instance;
            }
        }
    }
}
