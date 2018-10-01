using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class NodeScoreRequest : MonoBehaviour {

    void Start()
    {
        //StartCoroutine(inseret("3", "5", "1", "1"));
        //StartCoroutine(queryByCondition("5", "1", "3", "1522475047000", "", "1"));
        //StartCoroutine(queryByCondition("", "", "", "", "", ""));


        //long jsTimeStamp = 1522475047000;
        //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        //DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
        //Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));

    }

    public IEnumerator inseret(string nodeId, string lessonId, string studentId, string mode)//正确0，错误-1，跳过1，课程id是0，nodeId
    {
        JsonData data = new JsonData();
        data["nodeId"] = nodeId;
        data["lessonId"] = lessonId;
        data["studentId"] = studentId;
        data["mode"] = mode;


        string jsonDataPost = data.ToJson();
        Debug.LogWarning(jsonDataPost);

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";

        Debug.Log(UrlConf.url + "/nodeScore/insert");
        WWW www = new WWW(UrlConf.url + "/nodeScore/insert", Encoding.UTF8.GetBytes(jsonDataPost), headers);

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

    //以下所有参数都可以传""  传""后查询时忽略该项
    //timeFrom timeTo 格式都是 2018-03-27 00:00:00
    public IEnumerator queryByCondition(string lessonId = "", string studentId = "", string nodeId = "", string timeFrom = "", string timeTo = "", string mode = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("lessonId", lessonId);
        form.AddField("studentId", studentId);
        form.AddField("nodeId", nodeId);
        form.AddField("timeFrom", timeFrom);
        form.AddField("timeTo", timeTo);
        form.AddField("mode", mode);

        Debug.Log(UrlConf.url + "/nodeScore/queryByCondition");
        WWW www = new WWW(UrlConf.url + "/nodeScore/queryByCondition", form);

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
