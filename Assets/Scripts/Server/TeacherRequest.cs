using UnityEngine;
using LitJson;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class TeacherRequest : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(teacherLogin("t1", "1"));
        //StartCoroutine(teacherRegister("测试教师1", "t1", "1", 1, 1));
    }

    public IEnumerator teacherLogin (string userName, string password) {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("password", password);

        WWW postData = new WWW(UrlConf.url + "/teacher/login", form);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);
            //{ "id":9,"name":"刘老师","userName":"t1","password":"1","gradeId":"1","schoolId":1}

            JsonData data = JsonMapper.ToObject(postData.text);

            Teacher info = new Teacher();
            info.id = (int)data["id"];
            info.gradeId = (int)data["gradeId"];
            info.schoolId = (int)data["schoolId"];
            info.name = (string)data["name"];
            info.userName = (string)data["userName"];
            info.password = (string)data["password"];
        }
    }

    public IEnumerator teacherRegister(string teacherName, string loginName, string pass, int gradeId, int schoolId)
    {
        Teacher msgJson = new Teacher();
        //msgJson.name = "测试教师1";
        //msgJson.userName = "t1";
        //msgJson.password = "1";
        //msgJson.gradeId = "1";
        //msgJson.schoolId = "1";
        msgJson.name = teacherName;
        msgJson.userName = loginName;
        msgJson.password = pass;
        msgJson.gradeId = gradeId;
        msgJson.schoolId = schoolId;

        string jsonDataPost = JsonMapper.ToJson(msgJson);
        //Debug.Log(UrlConf.url + "/teacher/register");
        //Debug.Log(jsonDataPost);

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";

        WWW www = new WWW(UrlConf.url + "/teacher/register", Encoding.UTF8.GetBytes(jsonDataPost), headers);

        //while(!www.isDone)
        //{
        //    Debug.Log("wait");
        //}

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
                Debug.Log("注册成功");
            }
            else
            {
                Debug.Log("注册失败");
            }
        }
    }
}


public class Teacher {
    public int id;
    public string name;//教师姓名
    public string userName;//登录用户名
    public string password;//密码
    public int gradeId;//班级id
    public int schoolId;//学校id
}