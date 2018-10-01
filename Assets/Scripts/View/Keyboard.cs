using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.UI;

public class Keyboard : EventTriggerListener
{
    InputField input;
    InputField display;
    StringBuilder sb = new StringBuilder();
    List<Text> letters = new List<Text>();
    bool isShift = false;

    public override void Awake()
    {
        base.Awake();
        display = transform.Find("Panel/Vertical/Input").GetComponent<InputField>();
        Transform letter = transform.Find("Panel/Vertical/Letter");
        for (int i = 0; i < letter.childCount; i++)
        {
            letters.Add(letter.GetChild(i).Find("Text").GetComponent<Text>());
        }
    }

    public override void Show(object mObject)
    {
        base.Show(mObject);
        input = ((GameObject)mObject).GetComponent<InputField>();
        sb = new StringBuilder(input.text);
        display.text = sb.ToString();
    }

    void Shift(bool isUp)
    {
        isShift = isUp;
        foreach (Text letter in letters)
        {
            letter.text = isUp ? letter.text.ToUpper() : letter.text.ToLower();
        }
    }

    public override void OnClick()
    {
        base.OnClick();

        GameObject selectedGo = EventSystem.current.currentSelectedGameObject;
        if (selectedGo == null)
            return;

        if (selectedGo.name == "Background")
        {
            Enter();

            return;
        }

        if (selectedGo.name == "Shift")
        {
            Shift(true);
        }
        else
        {
            if (selectedGo.name == "Space")
            {
                sb.Append(" ");
            }
            else if (selectedGo.name == "Enter")
            {
                Enter();
            }
            else if (selectedGo.name == "Com")
            {
                sb.Append(".com");
            }
            else if (selectedGo.name == "Dot")
            {
                sb.Append(".");
            }
            else if (selectedGo.name == "Backspace")
            {
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
            }
            else
            {
                sb.Append(selectedGo.transform.Find("Text").GetComponent<Text>().text);
            }

            Back();
        }
    }

    private void Enter()
    {
        UIManager<Keyboard>.Hide();
        if (input is CustomInputField)
        {
            CustomInputField customInput = (CustomInputField)input;
            if (customInput.valueSummit != null)
                customInput.valueSummit();
        }
    }

    void Back()
    {
        if (isShift)
            Shift(false);

        display.text = sb.ToString();
        input.text = sb.ToString();
        if (input is CustomInputField)
        {
            CustomInputField customInput = (CustomInputField)input;
            if (customInput.valueChange != null)
                customInput.valueChange();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UIManager<Keyboard>.Hide();

            Back();
        }
    }
}
