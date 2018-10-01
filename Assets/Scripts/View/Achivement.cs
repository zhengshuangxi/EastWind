using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.Operation;
using Util.UI;

public class Achivement : EventTriggerListener
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    public void Back()
    {
        UIManager<Achivement>.Hide();
    }
}
