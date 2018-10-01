using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocol
{
    public int id;

    public Protocol()
    { }

    public Protocol(int id)
    {
        this.id = id;
    }

    public static Dictionary<int, System.Type> protocols = new Dictionary<int, System.Type>()
    {
        {1,typeof(HeartBeat)},
        {2,typeof(CourseStart )},
        {3,typeof(CourseFinish)},
        {4,typeof(CoursePause)},
        {5,typeof(CourseResume)},
        {6,typeof(SetVolume)},
        {7,typeof(ShutDown)},
        {8,typeof(NodeStart)},
        {9,typeof(NodeFinish)},
        {10,typeof(ControlStart)},
        {11,typeof(ControlFinish)},
        {12,typeof(Review)},
    };

    public static Protocol Parse(string data)
    {
        string[] ps = data.Split('|');
        int id;

        if (int.TryParse(ps[0], out id) && protocols.ContainsKey(id))
        {
            //Protocol protocol = LitJson.JsonMapper.ToObject < protocols[id] > (ps[1]);
            Protocol protocol = null;// =JsonUtility.FromJson<protocols[id]> (ps[1]);
            if (id == 1)
            {
                protocol = LitJson.JsonMapper.ToObject<HeartBeat>(ps[1]);
            }
            else if (id == 2)
            {
                protocol = LitJson.JsonMapper.ToObject<CourseStart>(ps[1]);
            }
            else if (id == 3)
            {
                protocol = LitJson.JsonMapper.ToObject<CourseFinish>(ps[1]);
            }
            else if (id == 4)
            {
                protocol = LitJson.JsonMapper.ToObject<CoursePause>(ps[1]);
            }
            else if (id == 5)
            {
                protocol = LitJson.JsonMapper.ToObject<CourseResume>(ps[1]);
            }
            else if (id == 6)
            {
                protocol = LitJson.JsonMapper.ToObject<SetVolume>(ps[1]);
            }
            else if (id == 7)
            {
                protocol = LitJson.JsonMapper.ToObject<ShutDown>(ps[1]);
            }
            else if (id == 8)
            {
                protocol = LitJson.JsonMapper.ToObject<NodeStart>(ps[1]);
            }
            else if (id == 9)
            {
                protocol = LitJson.JsonMapper.ToObject<NodeFinish>(ps[1]);
            }
            else if (id == 10)
            {
                protocol = LitJson.JsonMapper.ToObject<ControlStart>(ps[1]);
            }
            else if (id == 11)
            {
                protocol = LitJson.JsonMapper.ToObject<ControlFinish>(ps[1]);
            }
            else if (id == 12)
            {
                protocol = LitJson.JsonMapper.ToObject<Review>(ps[1]);
            }
            return protocol;
        }
        else
        {
            Debug.LogError("协议不识别：" + data);
            return null;
        }
    }
}

public class HeartBeat : Protocol
{
    public string vrNo;
    public int power;
    public int volume;

    public HeartBeat() { }
    public HeartBeat(string vrNo, int power, int volume, int id = 1) : base(id) { this.vrNo = vrNo; this.power = power; this.volume = volume; }
}

public class CourseStart : Protocol
{
    public int lessionId;

    public CourseStart() { }
    public CourseStart(int courseId, int id = 2) : base(id) { this.lessionId = courseId; }
}

public class CourseFinish : Protocol//一键退出和限时退出都调用这个
{
    public int lessionId;

    public CourseFinish() { }
    public CourseFinish(int courseId, int id = 3) : base(id) { this.lessionId = courseId; }
}

public class CoursePause : Protocol
{
    public CoursePause() { }
    public CoursePause(int id = 4) : base(id) { }
}

public class CourseResume : Protocol
{
    public CourseResume() { }
    public CourseResume(int id = 5) : base(id) { }
}

public class SetVolume : Protocol
{
    public int volume;

    public SetVolume() { }
    public SetVolume(int vol, int id = 6) : base(id) { this.volume = vol; }
}

public class ShutDown : Protocol
{
    public ShutDown() { }
    public ShutDown(int id = 7) : base(id) { }
}

public class NodeStart : Protocol
{
    public int nodeId;
    public string vrNo;

    public NodeStart() { }
    public NodeStart(int nodeId, string vrNo, int id = 8) : base(id) { this.nodeId = nodeId; this.vrNo = vrNo; }
}

public class NodeFinish : Protocol
{
    public int nodeId;
    public string vrNo;

    public NodeFinish() { }
    public NodeFinish(int nodeId, string vrNo, int id = 9) : base(id) { this.nodeId = nodeId; ; this.vrNo = vrNo; }
}

public class ControlStart : Protocol
{
    public int studentId;

    public List<ReviewRecord> knowledgeReviewList = new List<ReviewRecord>();
    public ControlStart() { }
    public ControlStart(int id = 10) : base(id) { }
}

public class ControlFinish : Protocol
{
    public ControlFinish() { }
    public ControlFinish(int id = 11) : base(id) { }
}

public class Review : Protocol
{
    public int studentId;
    public int pointId;
    public long ticks;
    public string vrNo;
    public bool active;

    //public ReviewRecord reviewRecord;

    public Review() { }
    public Review(int studentId, string vrNo,ReviewRecord reviewRecord, int id = 20) : base(id)
    {
        this.studentId = studentId;
        this.pointId = reviewRecord.pointId;
        this.ticks = reviewRecord.ticks;
        this.active = reviewRecord.active;
        this.vrNo = vrNo;
    }
}