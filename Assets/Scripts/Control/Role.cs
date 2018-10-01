#define NETMODE

using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



[Serializable]
public class CurrScore
{
    public string curriculums;
    public float value;
}

[Serializable]
public class RoleList
{
    public List<Role> roleList = new List<Role>();
}

[Serializable]
public class Role
{
    public int id = 0;
    public string userName;
    public string passWord;
    public long ticks;
    public bool isReview = false;
    public static string ip;
    public static int port;
    public static string sn;
    public static int power = 100;
    public static int volume = -1;
    public static bool isControl = false;
    public static string voiceName = string.Empty;

    [NonSerialized]
    public Dictionary<int, ReviewRecord> reviews = new Dictionary<int, ReviewRecord>();
    public List<ReviewRecord> reviewList = new List<ReviewRecord>();

    [NonSerialized]
    public Dictionary<string, float> scores = new Dictionary<string, float>();
    public List<CurrScore> scoreList = new List<CurrScore>();

    public static Role currentRole = null;
    public static RoleList roles = new RoleList();

    private static HashSet<string> builtInUsers = new HashSet<string>()
    {
        "30101",
        "30102",
        "30103",
        "30104",
        "30105",
        "30106",
        "30107",
        "30108",
        "30109",
        "30110",
        "30111",
        "30112",
        "30113",
        "30114",
        "30115",
        "30116",
        "30117",
        "30118",
        "30119",
        "30120",
        "30121",
        "30122",
        "30123",
        "30124",
        "30125",
        "30126",
        "30127",
        "30128",
        "30129",
        "30130",
        "30131",
        "a1",
        "a2",
        "a3",
        "a4",
        "a5",
        "a6",
        "a7",
        "a8",
        "a9",
        "a10",
        "a11",
        "a12",
        "a13",
        "a14",
        "a15",
        "a16",
        "a17",
        "a18",
        "a19",
        "a20",
        "a21",
        "a22",
        "a23",
        "a24",
        "a25",
        "a26",
        "a27",
        "a28",
        "a29",
        "a30",
        "a31",
        "a32",
        "a33",
        "a34",
        "a35",
        "a36",
        "a37",
        "a38",
        "a39",
        "a40",
        "a41",
        "a42",
        "a43",
        "a44",
        "a45",
        "a46",
        "a47",
        "a48",
        "a49",
        "a50",
        "a51",
        "a52",
        "a53",
        "a54",
        "a55",
        "a56",
        "a57",
        "a58",
        "a59",
        "a60",
    };

    public static void VoiceprintSuccess(string name)
    {
        voiceName = name;
        //currentRole = new Role();
        //currentRole.userName = name;
    }

    //public static void LoginTest(string name, string passWord)
    //{
    //    currentRole = new Role();
    //    currentRole.userName = name;
    //    currentRole.passWord = passWord;
    //    roles.roleList.Add(currentRole);
    //}

    public static void Register(string userName, string passWord, Action<int> registerResult)
    {
        if (!UserExist(userName))
        {
            if (passWord.Length < 6)
            {
                registerResult(2);
            }
            else
            {
                if (currentRole.userName == userName)
                {
#if NETMODE
                    Server.GetInstance().Register(userName, passWord, registerResult);
#else
                    registerResult(0);
#endif
                }
                else
                {
                    registerResult(3);
                }
            }
        }
        else
        {
            registerResult(1);
        }
    }

    //public static void RegisterResult(int result)
    //{

    //}

    public static void Login(string userName, Action<StudentInfo> loginResult)
    {
        for (int i = 0; i < roles.roleList.Count; i++)
        {
            if (roles.roleList[i].userName == userName)
            {
                currentRole = roles.roleList[i];
                currentRole.ticks = DateTime.Now.Ticks;
#if NETMODE
                Server.GetInstance().Login(currentRole.userName, currentRole.passWord, loginResult);
#else
                StudentInfo info = new StudentInfo();
                info.id = 0;
                loginResult(info);
#endif
                break;
            }
        }
    }

    public static void KeyboardLogin(string userName, string passWord, Action<int> loginResult)
    {
        Server.GetInstance().KeyboardLogin(userName, passWord, loginResult);
    }

    public static void VoiceLogin(string userName, Action<int> loginResult)
    {
        Server.GetInstance().VoiceLogin(userName, loginResult);
    }

    public static void NewRegister(string userName, string passWord, Action<int> registerResult)
    {
        Server.GetInstance().NewRegister(userName, passWord, registerResult);
    }

    public static void Login(string userName, string passWord, Action<StudentInfo> loginResult)
    {
        for (int i = 0; i < roles.roleList.Count; i++)
        {
            if (roles.roleList[i].userName == userName)
            {
                if (roles.roleList[i].passWord == passWord)
                {
                    currentRole = roles.roleList[i];
                    currentRole.ticks = DateTime.Now.Ticks;
#if NETMODE
                    Server.GetInstance().Login(currentRole.userName, currentRole.passWord, loginResult);
#else
                    StudentInfo info = new StudentInfo();
                    info.id = 0;
                    loginResult(info);
                    return;
#endif
                }
                else
                {
                    loginResult(null);
                    return;
                }
            }
        }
        if (builtInUsers.Contains(userName) && passWord == "123456")
        {
            currentRole = new Role();
            currentRole.userName = userName;
            currentRole.passWord = passWord;
            currentRole.ticks = DateTime.Now.Ticks;
            roles.roleList.Add(currentRole);
#if NETMODE
            Server.GetInstance().Login(currentRole.userName, currentRole.passWord, loginResult);
#else
            StudentInfo info = new StudentInfo();
            info.id = 0;
            loginResult(info);
#endif
            return;
        }
        loginResult(null);
    }

    public static void Exit()
    {
        for (int i = 0; i < roles.roleList.Count; i++)
        {
            Dictionary<int, ReviewRecord>.Enumerator iter1 = roles.roleList[i].reviews.GetEnumerator();
            while (iter1.MoveNext())
            {
                roles.roleList[i].reviewList.Add(iter1.Current.Value);
            }

            Dictionary<string, float>.Enumerator iter2 = roles.roleList[i].scores.GetEnumerator();
            while (iter2.MoveNext())
            {
                CurrScore score = new CurrScore();
                score.curriculums = iter2.Current.Key;
                score.value = iter2.Current.Value;
                roles.roleList[i].scoreList.Add(score);
            }
        }

        roles.roleList.Sort((Role x, Role y) =>
        {
            if (x.ticks < y.ticks)
                return -1;
            else if (x.ticks == y.ticks)
                return 0;
            else
                return 1;
        });

        string path = Application.persistentDataPath + "/user.json";
        string json = JsonUtility.ToJson(roles);

        File.WriteAllText(path, json);
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/user.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            roles = JsonUtility.FromJson<RoleList>(json);

            Debug.Log("RoleCount:" + roles.roleList.Count);

            for (int i = 0; i < roles.roleList.Count; i++)
            {
                for (int j = 0; j < roles.roleList[i].reviewList.Count; j++)
                {
                    if (!roles.roleList[i].reviews.ContainsKey(roles.roleList[i].reviewList[j].pointId))
                        roles.roleList[i].reviews.Add(roles.roleList[i].reviewList[j].pointId, roles.roleList[i].reviewList[j]);
                }

                for (int j = 0; j < roles.roleList[i].scoreList.Count; j++)
                {
                    if (!roles.roleList[i].scores.ContainsKey(roles.roleList[i].scoreList[j].curriculums))
                        roles.roleList[i].scores.Add(roles.roleList[i].scoreList[j].curriculums, roles.roleList[i].scoreList[j].value);
                }
            }
        }
    }

    public static bool UserExist(string userName)
    {
        for (int i = 0; i < roles.roleList.Count; i++)
        {
            if (roles.roleList[i].userName == userName)
            {
                return true;
            }
        }
        return false;
    }

    public static void RemoveFromReview()
    {

    }

    public static void AddToReviewByDialog(int sentenceId)
    {
        Dictionary<int, Point>.Enumerator iter = Point.points.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.sentenceId == sentenceId)
            {
                ReviewRecord reviewRecord;
                if (currentRole.reviews.ContainsKey(iter.Current.Value.id))
                {
                    reviewRecord = currentRole.reviews[iter.Current.Value.id];
                    reviewRecord.times += 1;
                }
                else
                {
                    reviewRecord = new ReviewRecord();
                    reviewRecord.times = 1;
                    reviewRecord.pointId = iter.Current.Value.id;
                    currentRole.reviews.Add(reviewRecord.pointId, reviewRecord);
                }
                //Server.GetInstance().Submit(iter.Current.Value.id,Role.currentRole.isReview);
                reviewRecord.ticks = GetReviewTicks(reviewRecord.times);
                Review review = new Review(Role.currentRole.id,Role.sn, reviewRecord);
                string msg = review.id + "|" + JsonMapper.ToJson(review);
                Debug.LogError(msg);
                (StudentLogin.GetInstance()).SendMsg(msg);
            }
        }
    }

    public static void AddToReview(int pointId)
    {
        //Server.GetInstance().Submit(pointId,false);
        ReviewRecord reviewRecord;
        if (currentRole.reviews.ContainsKey(pointId))
        {
            reviewRecord = currentRole.reviews[pointId];
            reviewRecord.times += 1;
        }
        else
        {
            reviewRecord = new ReviewRecord();
            reviewRecord.times = 1;
            reviewRecord.pointId = pointId;
            currentRole.reviews.Add(pointId, reviewRecord);
        }
        reviewRecord.ticks = GetReviewTicks(reviewRecord.times);

        reviewRecord.ticks = GetReviewTicks(reviewRecord.times);
        Review review = new Review(Role.currentRole.id,Role.sn, reviewRecord);
        string msg = review.id + "|" + JsonMapper.ToJson(review);
        Debug.LogError(msg);
        (StudentLogin.GetInstance()).SendMsg(msg);
    }

    public static long GetReviewTicks(int times)
    {
        DateTime now = DateTime.Now;
        if (times == 1)
        {

        }
        else if (times == 2)
        {
            now = now.AddMinutes(30);
        }
        else if (times == 3)
        {
            now = now.AddHours(2);
        }
        else if (times == 4)
        {
            now = now.AddDays(1);
        }
        else if (times == 5)
        {
            now = now.AddDays(2);
        }
        else if (times == 6)
        {
            now = now.AddDays(4);
        }
        else if (times == 7)
        {
            now = now.AddDays(7);
        }
        else
        {
            now = now.AddDays(15);
        }

        return now.Ticks;
    }
}