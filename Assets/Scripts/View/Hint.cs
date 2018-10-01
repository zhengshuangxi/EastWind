using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Hint : Singleton<Hint>
{
    TextMesh cn;
    TextMesh en;

    private void Awake()
    {
        cn = transform.Find("hint/cn").GetComponent<TextMesh>();
        en = transform.Find("hint/en").GetComponent<TextMesh>();
    }

    public void Show(string cnStr, string enStr)
    {
        cn.text = cnStr;
        en.text = enStr;
    }

    public void Hide()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
