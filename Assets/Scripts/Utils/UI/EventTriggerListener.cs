using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;
using Util.Operation;
using Util.UI;

public class EventTriggerListener : EventTrigger
{
    public virtual void Awake()
    {
        transform.GetComponent<Canvas>().worldCamera = GameObject.Find("Pvr_UnitySDK/Head").GetComponent<Camera>();
        RecursiveAddClickEvent(transform.gameObject, OnClick);
        RecursiveAddToggleEvent(transform.gameObject, OnToggle);
    }

    public void AddClickEvent(GameObject target, UnityAction callback)
    {
        Button button = target.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveListener(callback);
            button.onClick.AddListener(callback);
        }
    }

    private void RecursiveAddClickEvent(GameObject parent, UnityAction callback)
    {
        foreach (Transform child in parent.transform)
        {
            AddClickEvent(child.gameObject, callback);
            RecursiveAddClickEvent(child.gameObject, callback);
        }
    }

    public virtual void Show(object mObject) { }

    public virtual void OnClick()
    {
        Audio.GetInstance().Play(AudioType.OPER, "click");
    }

    public void AddToggleEvent(GameObject target, UnityAction<bool> callback)
    {
        Toggle toggle = target.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(callback);
            toggle.onValueChanged.AddListener(callback);
        }
    }

    private void RecursiveAddToggleEvent(GameObject parent, UnityAction<bool> callback)
    {
        foreach (Transform child in parent.transform)
        {
            AddToggleEvent(child.gameObject, callback);
            RecursiveAddToggleEvent(child.gameObject, callback);
        }
    }

    public virtual void OnToggle(bool isOn)
    {
        Audio.GetInstance().Play(AudioType.OPER, "click");
    }
}
