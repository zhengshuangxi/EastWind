using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;


public class StudentGetPostRequest : MonoBehaviour {

    private void Start()
    {
        //Score scoreData = new Score();
        //scoreData.studentId = 11;
        //scoreData.mode = 0;
        //scoreData.finishTime = 11.3f;
        //scoreData.lessonId = 1;
        //scoreData.useChineseSubtitles = false;
        //scoreData.useEnglishSubtitles = false;

        //List<KnowScore> list = new List<KnowScore>();
        //KnowScore know1 = new KnowScore();
        //know1.content = "hello";
        //know1.score = "2";
        //know1.usePromptPronunciation = false;
        //list.Add(know1);

        //KnowScore know2 = new KnowScore();
        //know2.content = "good";
        //know2.score = "5";
        //know2.usePromptPronunciation = true;
        //list.Add(know2);

        //scoreData.knowScoreList = list;

        //StartCoroutine(scoreInseret(scoreData));


        ////for(int i=30101;i<=30131;i++)
        ////{
        ////    StartCoroutine(studenttRegister("Linjing", i.ToString(), "123456", 3, 1,null));
        ////}



        //StartCoroutine(studentLogin("dawei", "1"));
    }

    void RegisterResult(int code)
    {
        Debug.Log(code);
    }

    public IEnumerator studentLogin(string userName, string password, Action<StudentInfo> loginResult)
    {
        Debug.Log("Server-StudentRegister Start:" + userName + " " + password);
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("password", password);

        WWW postData = new WWW(UrlConf.url + "/student/login", form);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
            loginResult(null);
            Debug.Log("Server-StudentRegister Failed:" + userName + " " + password);
        }
        else
        {
            Debug.Log(postData.text);
            //{"id":9,"name":"大维","userName":"dawei","password":"1","gradeId":2,"schoolId":1}

            JsonData data = JsonMapper.ToObject(postData.text);

            StudentInfo info = new StudentInfo();
            info.id = (int)data["id"];
            info.gradeId = (int)data["gradeId"];
            info.schoolId = (int)data["schoolId"];
            info.name = (string)data["name"];
            info.userName = (string)data["userName"];
            info.password = (string)data["password"];
            loginResult(info);
            Debug.Log("Server-StudentRegister Sucess:" +info.id+" "+ userName + " " + password);
        }
    }

    public IEnumerator scoreInseret(Score scoreInfo,Action<int> insertResult) {
        JsonData data = new JsonData();
        data["studentId"] = scoreInfo.studentId;
        data["mode"] = scoreInfo.mode;
        data["finishTime"] = scoreInfo.finishTime;
        data["lessonId"] = scoreInfo.lessonId;

        data["useChineseSubtitles"] = scoreInfo.useChineseSubtitles ? 0 : 1;
        data["useEnglishSubtitles"] = scoreInfo.useEnglishSubtitles ? 0 : 1;

        JsonData knowScoreList = new JsonData();
        foreach(KnowScore knowScore in scoreInfo.knowScoreList) {
            JsonData knowScoreData = new JsonData();
            knowScoreData["content"] = knowScore.content;
            knowScoreData["score"] = knowScore.score;
            knowScoreData["usePromptPronunciation"] = knowScore.usePromptPronunciation ? 0 : 1;

            knowScoreList.Add(knowScoreData);
        }

        data["knowScoreList"] = knowScoreList;
        string jsonDataPost = data.ToJson();//说明litjson是输出字符串了
        //Debug.Log(jsonDataPost);

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";

        //Debug.Log(UrlConf.url + "/score/insert");
        WWW www = new WWW(UrlConf.url + "/score/insert", Encoding.UTF8.GetBytes(jsonDataPost), headers);

        //while (!www.isDone)
        //{
        //    Debug.Log("wait");
        //}

        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
            insertResult(1);
        }
        else
        {
            Debug.Log(www.text);
            if (www.text == "0" || www.text == "success")
            {
                Debug.Log("提交成功");
                insertResult(0);
            }
            else
            {
                Debug.Log("提交失败");
                insertResult(1);
            }
        }
    }

    public IEnumerator KeyboardLogin(string userName, string password, Action<int> loginResult)
    {
        if (userName == "test" && password == "test")
        {
            loginResult(0);
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("userName", userName);
            form.AddField("password", password);

            WWW www = new WWW(UrlConf.url + "/student/login", form);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError(www.error);
                loginResult(-1);
            }
            else
            {
                loginResult(0);
            }
        }
    }

    public IEnumerator VoiceLogin(string userName, Action<int> loginResult)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);

        WWW www = new WWW(UrlConf.url + "/student/voiceLogin", form);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
            loginResult(-1);
        }
        else
        {
            loginResult(0);
        }
    }

    public IEnumerator NewRegister(string userName, string password, Action<int> registerResult)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("passWord", password);

        WWW www = new WWW(UrlConf.url + "/student/registerTmp", form);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
            registerResult(-1);
        }
        else
        {
            registerResult(0);
        }
    }

    public IEnumerator studenttRegister(string teacherName, string loginName, string pass, int gradeId, int schoolId,Action<int> registerResult)
    {
        Debug.Log("Server-StudentRegister Start:"+loginName+" "+pass);
        StudentInfo msgJson = new StudentInfo();
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

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";

        WWW www = new WWW(UrlConf.url + "/student/register", Encoding.UTF8.GetBytes(jsonDataPost), headers);

        //while (!www.isDone)
        //{ 
        //    Debug.Log("wait");
        //}

        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
            registerResult(1);
            Debug.Log("Server-StudentRegister Failed:" + loginName + " " + pass);
        }
        else
        {
            Debug.Log(www.text);
            if (www.text == "0" || www.text == "success")
            {
                Debug.Log("注册成功");
                registerResult(0);
                Debug.Log("Server-StudentRegister Success:" + loginName + " " + pass);
            }
            else
            {
                Debug.Log("注册失败");
                registerResult(1);
                Debug.Log("Server-StudentRegister Failed:" + loginName + " " + pass);
            }
        }
    }


    private Action<StudentStruct> _callBack = null;

    //没用
    public void studentQuery(int paseSize = 100, int currentPageIndex = 1, Action<StudentStruct> callBack = null) {
        _callBack = callBack;

        WWWForm form = new WWWForm();
        form.AddField("pageSize", paseSize);
        form.AddField("currentPageIndex", currentPageIndex);
        StartCoroutine(SendPost(UrlConf.url + "/student/query", form));
    }
	
    IEnumerator SendPost(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);

            JsonData data = JsonMapper.ToObject(postData.text);

            StudentStruct structInfo = new StudentStruct();
            structInfo.pageSize = (int)data["pageSize"];
            structInfo.currentPageIndex = (int)data["currentPageIndex"];
            structInfo.totalRecord = (int)data["totalRecord"];
            structInfo.totalPage = (int)data["totalPage"];

            if(data["datas"].IsArray) {
                List<StudentInfo> list = new List<StudentInfo>();
                foreach(JsonData datas in data["datas"]) {
                    //Debug.Log(datas["name"]);
                    StudentInfo info = new StudentInfo();
                    list.Add(info);

                    info.id = (int)datas["id"];
                    info.name = (string)datas["name"];
                    info.schoolId = (int)datas["schoolId"];
                    info.gradeId = (int)datas["gradeId"];
                    info.userName = (string)datas["userName"];
                    info.password = (string)datas["password"];
                }

                structInfo.gradeArr = list;
            }

            if(_callBack != null) {
                _callBack(structInfo);
            }
        }
    }
}

public class StudentStruct {
    public int pageSize;
    public int currentPageIndex;
    public int totalRecord;
    public int totalPage;
    public List<StudentInfo> gradeArr;
}

public class StudentInfo {
    public int id { get; set; }
    public string name { get; set; }
    public int schoolId { get; set; }
    public int gradeId { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
}

public class Score
{
    public int studentId;
    public int mode;//0 = 预习模式   1 = 学习模式
    public float finishTime;//学习时长，单位 分钟，例如学习了15分30秒 finishTime=15.5
    public int lessonId;//课程id
    public bool useChineseSubtitles;//是否试用了中文字幕 0=true  1=false
    public bool useEnglishSubtitles;//是否试用了英文字幕 0=true  1=false
    public List<KnowScore> knowScoreList;//每个知识点的得分情况
}

public class KnowScore
{
    public string content;//知识点内容
    public string score;//知识点得分
    public bool usePromptPronunciation;//是否使用提示
}