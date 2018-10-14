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
    List<Transform> answers = new List<Transform>();
    int count = 0;

    public void SetCallBack(Action<string> clickCallBack)
    {
        this.clickCallBack = clickCallBack;
    }

    public override void Awake()
    {
        base.Awake();

        for (int i = 1; i <= 7; i++)
        {
            Transform answer = transform.Find("Panel/Answer" + i);
            answer.gameObject.SetActive(false);
            answers.Add(answer);
        }
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

    public void ShowQuestion(List<string> answerContents, float clickOvertime)
    {
        count = answerContents.Count;

        for (int i = 0; i < answers.Count; i++)
        {
            if (i < answerContents.Count)
            {
                answers[i].gameObject.SetActive(true);
                answers[i].Find("Text").GetComponent<Text>().text = answerContents[i];
            }
            else
            {
                answers[i].gameObject.SetActive(false);
            }
        }

#if !Release
        Invoke("RandomSelect", clickOvertime);
#endif
    }

    public void RandomSelect()
    {
        if (count > 0)
        {
            int rand = UnityEngine.Random.Range(0, count);
            clickCallBack(answers[rand].Find("Text").GetComponent<Text>().text);
        }
    }
}
