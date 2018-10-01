using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class LessonRequest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StartCoroutine(inseret("课程2"));
        //StartCoroutine(queryByCondition("课程1"));
        StartCoroutine(queryById("5"));
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

        Debug.Log(UrlConf.url + "/lesson/insert");
        WWW www = new WWW(UrlConf.url + "/lesson/insert", Encoding.UTF8.GetBytes(jsonDataPost), headers);

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

        Debug.Log(UrlConf.url + "/lesson/queryByCondition");
        WWW www = new WWW(UrlConf.url + "/lesson/queryByCondition", form);

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

        Debug.Log(UrlConf.url + "/lesson/find");
        WWW www = new WWW(UrlConf.url + "/lesson/find", form);

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
