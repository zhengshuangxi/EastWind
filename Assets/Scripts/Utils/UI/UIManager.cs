using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Util.UI
{
    public class UIManager<T> : MonoBehaviour where T : EventTriggerListener
    {
        private static T instance;
        private static object locker = new object();
        private static GameObject parent;
        private static bool recoverParent = false;
        private static List<EventTriggerListener> enabledUI = new List<EventTriggerListener>();

        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        if (instance == null)
                        {
                            GameObject ui = Instantiate(Resources.Load("Panels/" + typeof(T).ToString()) as GameObject);
                            instance = ui.AddComponent<T>();
                        }
                    }
                    return instance;
                }
            }
        }

        public static void Show()
        {
            if (Instance != null)
                Instance.gameObject.SetActive(true);
            if (!enabledUI.Contains(Instance))
            {
                enabledUI.Add(Instance);
            }
        }

        public static void FollowShow(GameObject go)
        {
            Show();
            Instance.transform.SetParent(go.transform);
        }

        public static void Show(object mObject)
        {
            Instance.gameObject.SetActive(true);
            Instance.Show(mObject);
            if (!enabledUI.Contains(Instance))
            {
                enabledUI.Add(Instance);
            }
        }

        public static void Show(GameObject p, object mObject, bool recover = false)
        {
            recoverParent = recover;
            parent = p;
            parent.gameObject.SetActive(false);
            Show(mObject);
        }

        public static void Show(GameObject p, bool recover)
        {
            parent = p;
            recoverParent = recover;
            Show();
            parent.gameObject.SetActive(false);
        }

        public static void Hide(bool directly)
        {
            if (directly)
            {
                Instance.gameObject.SetActive(false);
            }
        }

        public static void Hide()
        {
            Instance.gameObject.SetActive(false);

            if (parent != null && recoverParent)
                parent.gameObject.SetActive(true);

            if (enabledUI.Contains(Instance))
            {
                enabledUI.Remove(Instance);
            }
        }

        public void AddClickEvent(GameObject target, UnityAction callback)
        {
            Button button = target.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(callback);
        }
    }
}
