using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util.Operation;
using Util.UI;

public class Login : EventTriggerListener
{
    CustomInputField userName;

    public override void Awake()
    {
        base.Awake();

        if (Role.currentRole == null)//首次登陆
        {
            userName = transform.Find("Panel/UserName").GetComponent<CustomInputField>();
            GameObject agent = Instantiate(Resources.Load("Models/Agent")) as GameObject;
            agent.name = "Agent";
        }
        else//从课程中返回
        {
            UIManager<Main>.Show(transform.gameObject,false);
        }
    }

    private void Start()
    {
        Hint.GetInstance().Show("欢迎来到光和空间！", "Welcome to Lightray+Space!");
        Audio.GetInstance().Play(AudioType.INTRO, "welcome");
}

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Voiceprint":
                Voiceprint();
                break;
            case "Register":
                Register();
                break;
            case "Keyboard":
                Keyboard();
                break;
            default:
                break;
        }
    }

    void Voiceprint()
    {
        UIManager<VoiceInput>.Show(transform.gameObject, userName.text, true);
    }

    void Register()
    {
        UIManager<Register>.Show(transform.gameObject, true);
    }

    void Keyboard()
    {
        UIManager<KeyInput>.Show(transform.gameObject, userName.text, true);
    }
}
