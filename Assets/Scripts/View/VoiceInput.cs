using UnityEngine;
using UnityEngine.EventSystems;
using Util.UI;

public class VoiceInput : EventTriggerListener
{
    CustomInputField userName;

    public override void Awake()
    {
        base.Awake();

        userName = transform.Find("Panel/UserName").GetComponent<CustomInputField>();
    }

    public override void Show(object mObject)
    {
        base.Show(mObject);
        userName.text = (string)mObject;
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
            case "Voice":
                Voice();
                break;
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        Hint.GetInstance().Show("点击按钮，说出\"芝麻开门\"!", "Click button, say \"芝麻开门\"!");
    }

    public void Voice()
    {
        if (string.IsNullOrEmpty(userName.text))
        {
            Hint.GetInstance().Show("用户名忘记输入了吧？", "Please input the user name!");
        }
        else
        {
            if (Role.UserExist(userName.text))
            {
                Hint.GetInstance().Show("请说出 \"芝麻开门\"!", "Please say \"芝麻开门\"!");
                GameObject.Find("Agent").GetComponent<Agent>().StartVerify(userName.text, "芝麻开门", VoiceprintSucess, VoiceprintFailed);
            }
            else
            {
                Hint.GetInstance().Show("用户不存在啊，注册一个新的吧！", "User is not exist!");
            }
        }
    }

    public void LoginSuccess()
    {
        UIManager<Main>.Show(transform.gameObject, false);
    }

    public void VoiceprintSucess()
    {

        //Role.Login(userName.text, LoginResult);
        Role.currentRole = new Role();
        Role.currentRole.userName = userName.text;
        Role.VoiceLogin(userName.text, LoginResult);
    }

    public void LoginResult(int code)
    {
        if(code==0)
        {
            Hint.GetInstance().Show("不错嘛，跟我来！", "Not bad, follow me!");
            LoginSuccess();
        }
        else
        {
            Hint.GetInstance().Show("用户名不存在啊！", "User is not exist!");
        }
    }

    public void LoginResult(StudentInfo si)
    {
        if (si != null)
        {
            Role.currentRole.id = si.id;
            Hint.GetInstance().Show("不错嘛，跟我来！", "Not bad, follow me!");
            LoginSuccess();
        }
    }

    public void VoiceprintFailed(string info)
    {
        Hint.GetInstance().Show("再说一遍，我没听清楚!", "Wrong!");
        GameObject.Find("Agent").GetComponent<Agent>().StartVerify(userName.text, "芝麻开门", VoiceprintSucess, VoiceprintFailed);
    }

    void Back()
    {
        UIManager<VoiceInput>.Hide();
    }
}
