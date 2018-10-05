using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util.Operation;
using Util.UI;

public class Question : EventTriggerListener
{
    Action<string> clickCallBack;

    public void SetCallBack(Action<string> clickCallBack)
    {
        this.clickCallBack = clickCallBack;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnClick()
    {
        base.OnClick();

        GameObject go = EventSystem.current.currentSelectedGameObject;
        if (go.transform.Find("Text") != null && clickCallBack != null)
        {
            clickCallBack(go.transform.Find("Text").GetComponent<Text>().text);
        }
    }
}
