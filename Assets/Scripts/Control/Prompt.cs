using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt
{
    public string En;
    public string Cn;
    public string Audio;

    public static List<Prompt> prompts = new List<Prompt>();

    public static void Load(string str)
    {
        string[] strs = str.Split(',');
        Prompt prompt = new Prompt();
        prompt.En = strs[0];
        prompt.Cn = strs[1];
        prompt.Audio = strs[2];
        prompts.Add(prompt);
    }

    public static Prompt Get()
    {
        return prompts[Random.Range(0, prompts.Count)];
    }
}
