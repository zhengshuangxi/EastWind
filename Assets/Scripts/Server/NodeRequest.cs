using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class NodeRequest : MonoBehaviour {

    void Start()
    {
        //StartCoroutine(inseret("节点a1"));
        //StartCoroutine(queryByCondition("节点a1"));
        StartCoroutine(queryById("3"));
    }

    public IEnumerator inseret(string lessonName)
    {
        JsonData data = new JsonData();
        data["name"] = lessonName;
        //data["id"] = "";


        string jsonDataPost = data.ToJson();
        Debug.Log(jsonDataPost);

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";

        Debug.Log(UrlConf.url + "/node/insert");
        WWW www = new WWW(UrlConf.url + "/node/insert", Encoding.UTF8.GetBytes(jsonDataPost), headers);

        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.text);
            if (www.text == "0" || www.text == "success")
            {
                Debug.Log("提交成功");
            }
            else
            {
                Debug.Log("提交失败");
            }
        }
    }

    public IEnumerator queryByCondition(string lessonName = "")//lessonName = ""查全部
    {
        WWWForm form = new WWWForm();
        form.AddField("name", lessonName);

        Debug.Log(UrlConf.url + "/node/queryByCondition");
        WWW www = new WWW(UrlConf.url + "/node/queryByCondition", form);

        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.text);

        }
    }

    public IEnumerator queryById(string lessonId)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", lessonId);

        Debug.Log(UrlConf.url + "/node/find");
        WWW www = new WWW(UrlConf.url + "/node/find", form);

        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.text);

        }
    }
}
