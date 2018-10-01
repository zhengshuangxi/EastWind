using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.Operation;
using Util.UI;

public class Register : EventTriggerListener
{
    const int SPEAK_TIMES = 5;

    CustomInputField userName;
    CustomInputField password;
    List<Transform> inputHints = new List<Transform>();

    Transform current = null;

    public override void Awake()
    {
        base.Awake();

        userName = transform.Find("Panel/UserName").GetComponent<CustomInputField>();
        userName.valueChange = UserNameChange;
        password = transform.Find("Panel/Password").GetComponent<CustomInputField>();
        password.valueChange = PasswordChange;
        for (int i = 1; i <= SPEAK_TIMES; i++)
        {
            inputHints.Add(transform.Find("Panel/Times/" + i.ToString()));
        }
    }

    void UserNameChange()
    {
        InputOK(userName.transform, !string.IsNullOrEmpty(userName.text) && !Role.UserExist(userName.text));
    }

    void PasswordChange()
    {
        InputOK(password.transform, password.text.Length >= 6);
    }

    void InputOK(Transform trans, bool ok)
    {
        trans.Find("NO").gameObject.SetActive(!ok);
        trans.Find("YES").gameObject.SetActive(ok);
    }

    public void OnEnable()
    {
        userName.text = "";
        password.text = "";

        InputOK(userName.transform, false);
        InputOK(password.transform, false);

        for (int i = 1; i <= SPEAK_TIMES; i++)
        {
            inputHints[i - 1].Find("YES").gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(userName.gameObject);
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Enter":
                RegisterEnter();
                break;
            case "Voice":
                RegisterVoiceprint();
                break;
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }
    public void RegisterEnter()
    {
        ////测试代码
        //Role.currentRole = new Role();
        //Role.currentRole.userName = userName.text;
        ////测试代码
        if (Role.voiceName == userName.text)
        {
            Role.currentRole = new Role();
            Role.currentRole.userName = userName.text;
            Role.NewRegister(userName.text, password.text, NewRegisterResult);
        }
        else
        {
            Hint.GetInstance().Show("声纹信息还没有录制！", "Voice print has not recorded!");
        }
        //Role.Register(userName.text, password.text, RegisterResult);
    }

    public void NewRegisterResult(int code)
    {
        if (code == 0)
        {
            Hint.GetInstance().Show("好了，让我们走！", "Well done, let's go!");
            RegisterSuccess();
        }
        else
        {
            Hint.GetInstance().Show("对不起，注册失败！", "Sorry,register failed!");
        }
    }

    public void RegisterResult(int code)//0=Success 1=UserName 2=Password 3=Voiceprint
    {
        if (code == 0)
        {
            Role.currentRole.passWord = password.text;
            Role.roles.roleList.Add(Role.currentRole);

            Hint.GetInstance().Show("好了，让我们走！", "Well done, let's go!");
            RegisterSuccess();
        }
        else if (code == 1)
        {
            Hint.GetInstance().Show("很遗憾，重复了！", "Sorry, name already exist!");
        }
        else if (code == 2)
        {
            Hint.GetInstance().Show("密码需要至少6位！", "Password is not less than 6 charactor!");
        }
        else if (code == 3)
        {
            Hint.GetInstance().Show("还没录入暗号呢！", "Voice print has not recorded!");
        }
    }

    void RegisterSuccess()
    {
        UIManager<Main>.Show(transform.gameObject, false);
    }

    public void RegisterVoiceprint()
    {
        if (!string.IsNullOrEmpty(userName.text))
        {
            //if (!Role.UserExist(userName.text))
            //{
                Hint.GetInstance().Show("请说出 \"芝麻开门\"！", "Password say \"芝麻开门\"!");
                GameObject.Find("Agent").GetComponent<Agent>().StartRegister(userName.text, "芝麻开门", Inputing, InputSuccess);
                TimesHint(0);
            //}
            //else
            //{
            //    Hint.GetInstance().Show("很遗憾，重复了！", "Sorry, name already exist!");
            //}
        }
        else
        {
            Hint.GetInstance().Show("用户名为空！", "User name is null!");
        }
    }

    public void Inputing(string info)
    {
        string[] splitTimes = info.Split(':');
        int speakTime = int.Parse(splitTimes[0]);

        if (speakTime < SPEAK_TIMES)
        {
            Hint.GetInstance().Show("请再次说出 \"芝麻开门\"！", "Say \"芝麻开门\" again!");
        }
        else
        {
            Hint.GetInstance().Show("好的，可以了！", "Ok, very good!");
        }

        TimesHint(speakTime);
    }

    public void InputSuccess(string vid)
    {
        Role.VoiceprintSuccess(userName.text);
        if (!string.IsNullOrEmpty(password.text))
            RegisterEnter();
    }

    public void TimesHint(int speakTime)
    {
        if (current != null)
        {
            current.Find("YES").GetComponent<Image>().color = Color.white;
            current = null;
        }

        if (speakTime < SPEAK_TIMES)
        {
            current = inputHints[speakTime];
            current.Find("YES").gameObject.SetActive(true);
        }
    }

    void Back()
    {
        UIManager<Register>.Hide();
    }

    private void Update()
    {
        if (current != null)
        {
            current.Find("YES").GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.realtimeSinceStartup % 1f, 255f));
        }
    }
}
