using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Util;

public class LoadData : MonoBehaviour
{
    delegate Dialog DialogLoader(string str);

    void Awake()
    {
        if (Role.currentRole == null)//首次登陆
        {
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        yield return StartCoroutine(Load("Scentences.csv", Sentence.Load));

        yield return StartCoroutine(Load("Points.csv", Point.Load));

        yield return StartCoroutine(Load("Curriculums.csv", Curriculum.Load));

        yield return StartCoroutine(Load("Prompts.csv", Prompt.Load));

        Dictionary<string, Curriculum>.Enumerator iter = Curriculum.curriculums.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.status == 1)
                yield return StartCoroutine(Load(iter.Current.Key + ".csv", iter.Current.Value, Dialog.Load));
        }
        Debug.Log("Load Done");
    }

    IEnumerator Load(string fileName, Action<string> loadAction)
    {
        string path = Path.GetStreamAssetsPath(fileName);
        WWW www = new WWW(path);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }

        string _result = Encoding.UTF8.GetString(www.bytes);
        string[] content = _result.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < content.Length; i++)
        {
            loadAction(content[i]);
        }
    }

    IEnumerator Load(string fileName, Curriculum cur, DialogLoader loadAction)
    {
        string path = Path.GetStreamAssetsPath(fileName);
        WWW www = new WWW(path);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }

        string _result = Encoding.UTF8.GetString(www.bytes);
        string[] content = _result.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < content.Length; i++)
        {
            cur.dialogs.Add(loadAction(content[i]));
        }
    }
}
