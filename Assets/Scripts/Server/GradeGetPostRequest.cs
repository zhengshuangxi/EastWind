using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class GradeGetPostRequest : MonoBehaviour {

    private Action<GradeStruct> _callBack = null;

    void Start() {
        gradeQuery();
    }

    public void gradeQuery(int paseSize = 100, int currentPageIndex = 1, Action<GradeStruct> callBack = null) {
        _callBack = callBack;

        WWWForm form = new WWWForm();
        form.AddField("pageSize", paseSize);
        form.AddField("currentPageIndex", currentPageIndex);
        StartCoroutine(SendPost(UrlConf.url + "/grade/query", form));
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

            GradeStruct structInfo = new GradeStruct();
            structInfo.pageSize = (int)data["pageSize"];
            structInfo.currentPageIndex = (int)data["currentPageIndex"];
            structInfo.totalRecord = (int)data["totalRecord"];
            structInfo.totalPage = (int)data["totalPage"];

            if(data["datas"].IsArray) {
                List<GradeInfo> list = new List<GradeInfo>();
                foreach(JsonData datas in data["datas"]) {
                    //Debug.Log(datas["name"]);
                    GradeInfo info = new GradeInfo();
                    list.Add(info);

                    info.id = (int)datas["id"];
                    info.name = (string)datas["name"];
                    info.schoolId = (int)datas["schoolId"];
                    info.teacherId = (int)datas["teacherId"];
                    info.schoolName = (string)datas["schoolName"];
                    info.teacherName = (string)datas["teacherName"];
                }

                structInfo.gradeArr = list;
            }

            if(_callBack != null) {
                _callBack(structInfo);
            }
        }
    }
}

public class GradeStruct {
    public int pageSize;
    public int currentPageIndex;
    public int totalRecord;
    public int totalPage;
    public List<GradeInfo> gradeArr;
}

public class GradeInfo {
    public int id { get; set; }
    public string name { get; set; }
    public int schoolId { get; set; }
    public int teacherId { get; set; }
    public string schoolName { get; set; }
    public string teacherName { get; set; }
}