using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sentence
{
    public int id;
    public string en;
    public string cn;
    public string audio;
    public string itemName;
    public string anim;

    public static Dictionary<int, Sentence> sentences = new Dictionary<int, Sentence>();

    public static void Load(string str)
    {
        string[] strs = str.Split(',');
        Sentence sen = new Sentence();
        sen.id = int.Parse(strs[0]);
        sen.en = strs[1].Replace('|', ',');
        sen.cn = strs[2];
        sen.audio = strs[3];
        sen.itemName = strs[4];
        sen.anim = strs[5];
        sentences.Add(sen.id, sen);
    }

    public static Sentence Get(int id)
    {
        if (sentences.ContainsKey(id))
            return sentences[id];
        return null;
    }
}
