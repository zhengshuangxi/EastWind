using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog
{
    public int triggerId;
    public int askId;
    public List<int> answerIds;
    public string keyword;
    public int transfer;//1=移动镜头

    public static Dialog Load(string str)
    {
        string[] strs = str.Split(',');
        Dialog dialog = new Dialog();
        if (!string.IsNullOrEmpty(strs[0]))
            dialog.triggerId = int.Parse(strs[0]);
        if (!string.IsNullOrEmpty(strs[1]))
            dialog.askId = int.Parse(strs[1]);
        else
            dialog.askId = -1;
        string[] answers = strs[2].Split('|');
        dialog.answerIds = new List<int>();
        if (!string.IsNullOrEmpty(strs[2]))
        {
            for (int i = 0; i < answers.Length; i++)
            {
                dialog.answerIds.Add(int.Parse(answers[i]));
            }
        }
        dialog.keyword = strs[3];
        if (!string.IsNullOrEmpty(strs[4]))
            dialog.transfer = int.Parse(strs[4]);
        return dialog;
    }
}
