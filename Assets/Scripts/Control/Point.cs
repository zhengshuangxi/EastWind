using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Point
{
    public int id;
    public int sentenceId;
    public string en;
    public string cn;
    public string audio;
    public string desc;

    public static Dictionary<int, Point> points = new Dictionary<int, Point>();

    public static void Load(string str)
    {
        string[] strs = str.Split(',');
        Point point = new Point();
        point.id = int.Parse(strs[0]);
        point.sentenceId = int.Parse(strs[1]);
        point.en = strs[2];
        point.cn = strs[3];
        point.audio = strs[4];
        point.desc = strs[5];
        points.Add(point.id, point);
    }

    public static List<string> GetConfusionAnswers(int id)
    {
        HashSet<int> hashSets = new HashSet<int>() { id };
        List<string> answers = new List<string>();

        while (answers.Count < 3)
        {
            int random = UnityEngine.Random.Range(1, points.Count + 1);
            if (!hashSets.Contains(random))
            {
                answers.Add(points[random].cn);
                hashSets.Add(random);
            }
        }

        return answers;
    }
}
