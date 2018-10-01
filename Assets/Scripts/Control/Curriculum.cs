using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Curriculum
{
    public string enName;
    public string cnName;
    public int status;
    public List<Dialog> dialogs;

    public static Dictionary<string, Curriculum> curriculums = new Dictionary<string, Curriculum>();

    public static void Load(string str)
    {
        string[] strs = str.Split(',');
        Curriculum cur = new Curriculum();
        cur.enName = strs[0];
        cur.cnName = strs[1];
        cur.status = int.Parse(strs[2]);
        cur.dialogs = new List<Dialog>();
        curriculums.Add(cur.enName,cur);
    }

    public static Curriculum Get(string name)
    {
        if(curriculums.ContainsKey(name))
        {
            return curriculums[name];
        }

        return null;
    }
}
