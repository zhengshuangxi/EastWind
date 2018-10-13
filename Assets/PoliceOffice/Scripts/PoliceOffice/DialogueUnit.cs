using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 对话单元
/// </summary>
public class DialogueUnit {

    private bool isDone = false;
    private int count = 0;

    public bool IsDone
    {
        get { return isDone; }
        set { isDone = value; }
    }

    public int Count
    {
        get { return count; }
        set { count = value; }
    }

    public virtual void Refresh(Action<bool, string> singleEvent = null, Action<float, List<string>> callback = null)
    {

    }
}

public class OperateUnit : DialogueUnit
{
    private string ask;
    private string hint;
    private string anim;
    private string audioName;
    private string answer;
    private float limit;//回答时间，默认为-1不限制
    private bool isHaveBubble;//是否有气泡提示
    private string bubble;//气泡名称,默认无气泡提示
    private bool isOpen = false;
    private float currLimit;
    private float score;

    public string Ask { get { return ask; } }

    public string Hint { get { return hint; } }

    public string Anim { get { return anim; } }

    public string AudioName { get { return audioName; } }

    public bool IsHaveBubble { get { return isHaveBubble; } }

    public string Bubble { get { return bubble; } }

    private Agent agent;

    private List<float> scores = new List<float>();
    private List<string> letters = new List<string>();

    public OperateUnit()
    {
        
    }

    public OperateUnit(Agent agent, string ask, string hint, string anim, string audioName, string answer, float score, bool isHaveBubble, string bubble = null, float limit = -1)
    {
        this.agent = agent;
        this.ask = ask;
        this.hint = hint;
        this.anim = anim;
        this.audioName = audioName;
        this.answer = answer.ToLower();
        this.limit = limit;
        this.isHaveBubble = isHaveBubble;
        this.bubble = bubble;
        currLimit = limit;
        this.score = score;
    }

    public OperateUnit(Agent agent, string content)
    {
        this.agent = agent;
        string[] arr = content.Split(',');
        ask = arr[1];
        hint = arr[2];
        anim = arr[3];
        audioName = arr[4];
        answer = arr[5].ToLower();
        if (arr[6].Equals("t") || arr[6].Equals("T"))
        {
            isHaveBubble = true;
            bubble = arr[7];
        }
        else
        {
            isHaveBubble = false;
            bubble = null;
        }

        limit = float.Parse(arr[8]);
        currLimit = limit;
        score = float.Parse(arr[9]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="singleEvent"></param>
    /// <param name="callback">当前步骤完成的回调</param>
    public override void Refresh(Action<bool, string> singleEvent = null, Action<float, List<string>> callback = null)
    {
        if (limit == -1)
        {
            //do something
            return;
        }
        limit -= Time.deltaTime;
        if (limit <= 0)
        {
            //超时提醒,关闭识别和评测
            Debug.Log("超时提醒！");          
            float time = Audio.GetInstance().Play(AudioType.INTRO, "hint2");
            agent.StartCoroutine(WaitForSth(time, () => {
                isOpen = false;
                limit = currLimit;
            }));
            limit = currLimit;
        }
        else
        {
            //开启识别与评测
            //Debug.Log("开启识别与评测");
            if (!isOpen)
            {
                isOpen = true;
                if (Count < 3)
                {
                    Count++;
                    agent.StartRecognize((text) =>
                    {
                        Debug.Log("识别结果:" + text);
                        if (string.IsNullOrEmpty(text)) return;
                        //if (singleEvent != null)
                        //    singleEvent();
                        string result = text.ToLower();
                        if (answer.Contains("|"))
                        {
                            string[] arr = answer.Split('|');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (arr[i].Contains("&"))
                                {
                                    ParseAnswer(callback, arr[i], result);
                                }
                                else if (result.Contains(arr[i]))
                                {
                                    //直接传过去当前分数 score
                                    callback(score, letters);
                                    break;
                                }
                            }
                        }
                        else if (answer.Contains("&"))
                        {
                            ParseAnswer(callback, answer, result);
                        }
                        else
                        {
                            if (result.Contains(answer))
                                callback(score, letters);
                        }
                    });
                    if (singleEvent != null)
                        singleEvent(isHaveBubble, bubble);
                }
                else//3次之后直接给过
                {
                    //取三次回答的最大值 scorces[0]
                    float currScore = 0;
                    if (scores.Count > 0)
                    {
                        scores.Sort((p, q) => q.CompareTo(p));
                        currScore = scores[0];
                    }
                    callback(currScore, letters);
                    scores.Clear();
                    letters.Clear();
                }
            }
        }
    }

    //解析传入的内容content
    private void ParseAnswer(Action<float, List<string>> callback, string content, string text)
    {
        string[] arr = content.Split('&');
        int sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (!text.Contains(arr[i]))
            {
                letters.Add(arr[i]);
                break;
            }
            sum++;
        }
        scores.Add(sum * (score / arr.Length));//存储当前点数
        if (sum == arr.Length)
            callback(score, letters);//直接传分数
    }

    void ReceiveEvaluatorResult(string content)
    {
        Debug.LogError("EvaluatorResult");

        Result result = XmlParser.Parse(content);

        if (result.error == Error.NORMAL)
        {
            Debug.LogError("EvaluatorResult Error.NORMAL");
            float time = 0.5f;

            if (result.score.total > 4)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "perfect");
            }
            else if (result.score.total > 3)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "great");
            }
            else if (result.score.total > 2)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "good");
            }
            else
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "comeon");
            }

            agent.StartCoroutine(WaitForSth(time, () => { IsDone = true; }));

        }
        else
        {
            Debug.LogError("EvaluatorResult Error.Other");
            agent.StartEvaluator(ReceiveEvaluatorResult, content);
        }
    }

    IEnumerator WaitForSth(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
