using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Util.UI;

public class StudentLogin : Singleton<StudentLogin>
{
    //SystemInfo.deviceUniqueIdentifier
    // Use this for initialization
    //void Start()
    //{

    //}ClientSocket mSocket = new ClientSocket();  
    ClientSocket mSocket = null;
    ServerSocket serverSocket = null;

    //private const int port = 2021;
    //private static string IpStr = "127.0.0.1";
    //private static Socket serverSocket;
    public string sn;
    public float deltaTime = 1f;

    Agent agent;

    float time;
    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        time = Time.timeSinceLevelLoad;
        agent = transform.GetComponent<Agent>();
    }

    public IEnumerator Start()
    {
        Role.sn = agent.GetDeviceSN();
        //SystemInfo.deviceUniqueIdentifier
        Debug.LogError("SystemInfo:" + Role.sn);
        string url = "http://120.78.214.169/study/api/server/login?vrno=" + Role.sn + "&port=2021";
        WWW postData = new WWW(url);
        yield return postData;
        if (postData.error != null)
        {
            //Debug.Log(postData.error);
            //loginResult(null);
            //Debug.Log("Server-StudentRegister Failed:" + userName + " " + password);
            Debug.Log("Error, postData" + postData.error);
        }
        else
        {
            Debug.Log(postData.text);
            //{"id":9,"name":"大维","userName":"dawei","password":"1","gradeId":2,"schoolId":1}

            JsonData data = JsonMapper.ToObject(postData.text);

            //StudentInfo info = new StudentInfo();
            //info.id = (int)data["id"];
            //info.gradeId = (int)data["gradeId"];
            //info.schoolId = (int)data["schoolId"];
            //info.name = (string)data["name"];
            //info.userName = (string)data["userName"];
            //info.password = (string)data["password"];
            Role.ip = (string)data["server_ip"];
            Role.port = (int)data["server_port"];

            mSocket = new ClientSocket();
            mSocket.ConnectServer(Role.ip, Role.port, ReceiveMsg);

            serverSocket = new ServerSocket();
            //mSocket.SendMessage("服务器傻逼！");
            // loginResult(info);
            //Debug.Log("Server-StudentRegister Sucess:" + info.id + " " + userName + " " + password);

            // InvokeRepeating("Run", 0, 1f);
        }
    }

    //public void StartServer()
    //{
    //    IPAddress ip = IPAddress.Parse(IpStr);
    //    IPEndPoint ip_end_point = new IPEndPoint(ip, port);
    //    //创建服务器Socket对象，并设置相关属性  
    //    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //    //绑定ip和端口  
    //    serverSocket.Bind(ip_end_point);
    //    //设置最长的连接请求队列长度  
    //    serverSocket.Listen(10);
    //}

    public void ReceiveMsg(string data)
    {
        //data = data.TrimStart(' ');
        Debug.Log("ReceiveMsg:" + data);
        if (data.Contains("|"))
        {
            Protocol protocol = Protocol.Parse(data);
            if (protocol == null)
                return;

            if (protocol.id == 1)
            {
                //Debug.Log("HeartBeat");
            }
            else if (protocol.id == 2)
            {
                Debug.Log("Course Start");
                int courseId = ((CourseStart)protocol).lessionId;
                if (courseId == 4)
                {
                    Course.Story();
                }
                else if (courseId == 5)
                {
                    Course.Dinner();
                }
                else if (courseId == 6)
                {
                    Course.Stroll();
                }
            }
            else if (protocol.id == 3)
            {
                Debug.Log("Course Finish");
                if (SceneManager.GetActiveScene().name == "Main" || SceneManager.GetActiveScene().name == "Loading")
                {

                }
                else
                {
                    Server.GetInstance().FinishStudy();
                    Loading.scene = "Main";
                    SceneManager.LoadScene("Loading");
                }
            }
            else if (protocol.id == 4)
            {
                Debug.Log("Course Pause");
                Time.timeScale = 0f;
            }
            else if (protocol.id == 5)
            {
                Time.timeScale = 1f;
                Debug.Log("Course Resume");
            }
            else if (protocol.id == 6)
            {
                Debug.Log("SetVolume:" + ((SetVolume)protocol).volume);
                agent.SetVolume(((SetVolume)protocol).volume);
            }
            else if (protocol.id == 7)
            {
                Debug.Log("ShutDown");
                agent.ShutDown();
            }
            else if (protocol.id == 10)
            {
                // Debug.Log("Control Start");
                GameObject go = GameObject.Find("Login");
                if (null != go)
                    UIManager<Main>.Show(go, false);
                else
                {
                    go = GameObject.Find("VoiceInput(Clone)");
                    if (null != go)
                        UIManager<Main>.Show(go, false);
                    else
                    {
                        go = GameObject.Find("KeyInput(Clone)");
                        if (null != go)
                            UIManager<Main>.Show(go, false);
                    }
                }
                Role.currentRole = new Role();
                Role.currentRole.id = ((ControlStart)protocol).studentId;
                Role.currentRole.reviewList = ((ControlStart)protocol).knowledgeReviewList;
                if (Role.currentRole.reviewList == null)
                    Role.currentRole.reviewList = new List<ReviewRecord>();
                for (int i=0;i<Role.currentRole.reviewList.Count;i++)
                {
                    Role.currentRole.reviews.Add(Role.currentRole.reviewList[i].pointId, Role.currentRole.reviewList[i]);
                }
                Role.isControl = true;
            }
            else if (protocol.id == 11)
            {
                Debug.Log("Control Finish");
                Role.isControl = false;
            }
        }
    }

    public void Update()
    {
        deltaTime -= Time.unscaledDeltaTime;
        if (deltaTime <= 0 && mSocket != null)
        {
            deltaTime = 1f;
            if (Role.volume < 0)
            {
                Role.volume = agent.GetVolume();
                Role.power = agent.GetBattery();
            }
            HeartBeat heartBeat = new HeartBeat(Role.sn, Role.power, Role.volume);
            mSocket.SendMessage(heartBeat.id + "|" + JsonMapper.ToJson(heartBeat));
            mSocket.ReceiveMsg();
        }
    }

    public void SendMsg(string msg)
    {
        if (mSocket != null)
        {
            mSocket.SendMessage(msg);
        }
    }

    //public void ServerRun()
    //{
    //    Socket clientSocket = serverSocket.Accept();
    //    Console.WriteLine("客户端{0}成功连接", clientSocket.RemoteEndPoint.ToString());
    //    //向连接的客户端发送连接成功的数据  
    //    ByteBuffer buffer = new ByteBuffer();
    //    buffer.WriteString("Connected Server");
    //    clientSocket.Send(WriteMessage(buffer.ToBytes()));
    //    //每个客户端连接创建一个线程来接受该客户端发送的消息  
    //    Thread thread = new Thread(RecieveMessage);
    //    thread.Start(clientSocket);
    //}

    //public void SendMessage(string data)
    //{
    //    if (IsConnected == false)
    //        return;
    //    try
    //    {
    //        ByteBuffer buffer = new ByteBuffer();
    //        buffer.WriteString(data);
    //        clientSocket.Send(WriteMessage(buffer.ToBytes()));
    //    }
    //    catch
    //    {
    //        IsConnected = false;
    //        clientSocket.Shutdown(SocketShutdown.Both);
    //        clientSocket.Close();
    //    }
    //}

    ///// <summary>  
    ///// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据  
    ///// </summary>  
    ///// <param name="message"></param>  
    ///// <returns></returns>  
    //private static byte[] WriteMessage(byte[] message)
    //{
    //    MemoryStream ms = null;
    //    using (ms = new MemoryStream())
    //    {
    //        ms.Position = 0;
    //        BinaryWriter writer = new BinaryWriter(ms);
    //        ushort msglen = (ushort)message.Length;
    //        writer.Write(msglen);
    //        writer.Write(message);
    //        writer.Flush();
    //        return ms.ToArray();
    //    }
    //}
}
