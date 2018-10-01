using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.Operation;
using Util.UI;

public class KeyInput : EventTriggerListener
{
    CustomInputField userName;
    CustomInputField password;

    public override void Awake()
    {
        base.Awake();

        userName = transform.Find("Panel/UserName").GetComponent<CustomInputField>();
        password = transform.Find("Panel/Password").GetComponent<CustomInputField>();
    }

    public override void Show(object mObject)
    {
        base.Show(mObject);
        userName.text = (string)mObject;
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Enter":
                Keyboard();
                break;
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    public void Keyboard()
    {
        if (string.IsNullOrEmpty(userName.text))
        {
            Hint.GetInstance().Show("用户名忘记输入了吧？", "Please input the user name!");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            Hint.GetInstance().Show("密码为空吧？", "Please input the password!");
        }
        else
        {
            Login();
        }
    }

    void Login()
    {
        //Role.Login(userName.text, password.text, LoginResult);

        Role.currentRole = new Role();
        Role.currentRole.userName = userName.text;
        Role.KeyboardLogin(userName.text, password.text, LoginResult);
    }

    void LoginResult(int code)
    {
        if(code==0)
        {
            Hint.GetInstance().Show("不错嘛，跟我来！", "Not bad, follow me!");
            LoginSuccess();
        }
        else
        {
            Hint.GetInstance().Show("别想玩我，这个人根本不存在！", "The user is not exist!");
        }
    }

    void LoginResult(StudentInfo si)
    {
        if (si != null)
        {
            Role.currentRole.id = si.id;
            Hint.GetInstance().Show("不错嘛，跟我来！", "Not bad, follow me!");
            LoginSuccess();
        }
        else //if (code == 1)
        {
            Hint.GetInstance().Show("别想玩我，这个人根本不存在！", "The user is not exist!");
        }
        //else if (code == 2)
        //{
        //    Hint.GetInstance().Show("别想蒙混过关，你的口令根本不对！", "The password is wrong!");
        //}
    }

    public void Back()
    {
        UIManager<KeyInput>.Hide();
    }

    public void LoginSuccess()
    {
        UIManager<Main>.Show(transform.gameObject, false);
    }
}
