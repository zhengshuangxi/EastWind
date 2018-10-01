using System;
using UnityEngine;
using Util;

public class Server : Singleton<Server>
{
    GradeGetPostRequest ggpr;
    StudentGetPostRequest sgpr;
    NodeScoreRequest nsr;
    //StudentLogin sl;
    static Score score = null;
    static DateTime startTime;
    static DateTime finishTime;

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);

        //ggpr = this.transform.gameObject.AddComponent<GradeGetPostRequest>();
        sgpr = this.transform.gameObject.AddComponent<StudentGetPostRequest>();
        nsr = this.transform.gameObject.AddComponent<NodeScoreRequest>();
       // sl = this.transform.gameObject.AddComponent<StudentLogin>();
    }

    public void Login(string userName, string passWord, Action<StudentInfo> loginResult)
    {
        StartCoroutine(sgpr.studentLogin(userName, passWord, loginResult));
    }

    public void Register(string userName, string password, Action<int> registerResult, string teacherName = "Test", int gradeId = 1, int schoolId = 1)
    {
        StartCoroutine(sgpr.studenttRegister(teacherName, userName, password, gradeId, schoolId, registerResult));
    }

    public void KeyboardLogin(string userName,string passWord,Action<int> loginResult)
    {
        StartCoroutine(sgpr.KeyboardLogin(userName, passWord, loginResult));
    }

    public void VoiceLogin(string userName, Action<int> loginResult)
    {
        StartCoroutine(sgpr.VoiceLogin(userName, loginResult));
    }

    public void NewRegister(string userName, string password, Action<int> registerResult)
    {
        StartCoroutine(sgpr.NewRegister(userName, password, registerResult));
    }

    public void StartStudy(int lessonId)
    {
        Debug.Log("StartStudy:" + lessonId);
        startTime = DateTime.Now;
        score = new Score();
        score.knowScoreList = new System.Collections.Generic.List<KnowScore>();
        score.lessonId = lessonId;
        score.mode = Role.currentRole.isReview ? 1 : 0;
        score.studentId = Role.currentRole.id;
        score.useChineseSubtitles = !Role.currentRole.isReview;
        score.useEnglishSubtitles = true;
    }

    public void Studying(Sentence sentence, float sc)
    {
        Debug.Log("Studying:" + sentence.en);

        KnowScore ks = new KnowScore();
        ks.content = sentence.en;
        ks.score = sc.ToString();
        ks.usePromptPronunciation = false;
        score.knowScoreList.Add(ks);
    }

    public void Studying(string content, float sc)
    {
        Debug.LogError("Studying:"+content+":"+sc.ToString());
        KnowScore ks = new KnowScore();
        ks.content = content;
        ks.score = sc.ToString();
        ks.usePromptPronunciation = false;
        score.knowScoreList.Add(ks);
    }

    public void FinishStudy()
    {
        Debug.LogError("FinishStudy");
        finishTime = DateTime.Now;
        if (score != null)
        {
            score.finishTime = (finishTime - startTime).Seconds;
            StartCoroutine(sgpr.scoreInseret(score, SubmitResult));
        }
    }

    public void InsertNode(int nodeId, int mode)
    {
        if (nodeId >= 0)
        {
            Debug.LogError("InsertNode:" + nodeId + " " + score.lessonId + " " + score.studentId + " " + mode);
            StartCoroutine(nsr.inseret(nodeId.ToString(), score.lessonId.ToString(), score.studentId.ToString(), mode.ToString()));
        }
    }

    public void SubmitResult(int code)
    {
        if (code == 1)
        {
            Debug.Log("SubmitResult Failed");
            StartCoroutine(sgpr.scoreInseret(score, SubmitResult));
        }
        else
        {
            Debug.Log("SubmitResult Sucess");
        }
    }
}
