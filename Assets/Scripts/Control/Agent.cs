using System;
using UnityEngine;
using Util;

public class Agent : Singleton<MonoBehaviour>
{
    private AndroidJavaObject instance = null;
    private Action verifySuccess;
    private Action<string> verifyFailed;
    private Action<string> rigisterSuccess;
    private Action<string> rigistering;
    private Action<string> evaluatorResult;
    private Action<string> recognizeResult;
    private Transform netWork; //android.os.Build.SERIAL
    private string result;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        netWork = transform.Find("Network");
        netWork.gameObject.SetActive(false);

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass voice = new AndroidJavaClass("com.weolcen.voice.MainActivity");

            if (voice != null && currentActivity != null)
            {
                instance = voice.CallStatic<AndroidJavaObject>("GetInstance");
                instance.Call("SetContext", currentActivity, "Agent");
            }
        }
    }

    public void StartRecognize(Action<string> recognizeResult, string result = "", float overTime = 5f)
    {
        Debug.Log("StartRecongnize:" + DateTime.Now.ToLongTimeString());
        this.recognizeResult = recognizeResult;
        this.result = result;
#if Release
        if (instance != null)
            instance.Call("StartRecognize");
#else
        Invoke("RecognizeResultCallBack", overTime);
#endif
    }

    void RecognizeResultCallBack()
    {
        RecognizeResult(result);
    }

    public string GetDeviceSN()
    {
        if (instance != null)
            return instance.Call<string>("GetDeviceSN");
        return "PA7210DGB6200482G";
    }

    public int GetVolume()
    {
        if (instance != null)
            return int.Parse(instance.Call<string>("GetVolume"));
        return 100;
    }

    void StopRecognize()
    {
        Debug.Log("StopRecongnize:" + DateTime.Now.ToLongTimeString());
        if (instance != null)
            instance.Call("StopRecognize");
    }

    public void SetVolume(int vol)
    {
        Debug.Log("SetVolume:" + vol);
        if (instance != null)
            instance.Call("SetVolume", vol);
    }

    public void ShutDown()
    {
        if (instance != null)
            instance.Call("ShutDown");
    }

    void VolumeChange(string text)
    {
        Debug.Log("VolumeChange:" + text);
        Role.volume = int.Parse(text);
    }

    void BatteryChange(string text)
    {
        Debug.Log("BatteryChange:" + text);
        Role.power = int.Parse(text);
    }

    public int GetBattery()
    {
        if (instance != null)
            return int.Parse(instance.Call<string>("GetBattery"));
        return 100;
    }

    public void StartEvaluator(Action<string> evaluatorResult, string text, float overTime = 5f)
    {
        Debug.Log("StartEvaluator:" + DateTime.Now.ToLongTimeString() + text);
        this.evaluatorResult = evaluatorResult;
#if Release
        if (instance != null)
        {
            instance.Call("StartEvaluator", text);
        }
#else
        Invoke("EvaluatorResultCallBack", overTime);
#endif
    }

    void EvaluatorResultCallBack()
    {
        EvaluatorResult("");
    }

    void StartEvaluator(string text)
    {
        Debug.Log("StartEvaluator:" + DateTime.Now.ToLongTimeString() + text);
        if (instance != null)
            instance.Call("StartEvaluator", text);
    }

    void StopEvaluator()
    {
        Debug.Log("StopEvaluator:" + DateTime.Now.ToLongTimeString());
        if (instance != null)
            instance.Call("StopEvaluator");
    }

    void CancelEvaluator()
    {
        if (instance != null)
            instance.Call("CancelEvaluator");
    }

    void RecognizeVolumeChanged(string vol)
    {

    }

    void RecognizeResult(string result)
    {
        if (recognizeResult != null)
        {
            recognizeResult(result);
            recognizeResult = null;
        }
    }

    void EvaluatorVolumeChanged(string vol)
    {

    }

    void EvaluatorResult(string result)
    {
        Debug.Log("EvaluatorResult");
        if (evaluatorResult != null)
        {
            evaluatorResult(result);
            evaluatorResult = null;
        }
    }

    public void StartRegister(string userName, string passWord, Action<string> rigistering, Action<string> rigisterSuccess)//password为声纹的文本
    {
        this.rigistering = rigistering;
        this.rigisterSuccess = rigisterSuccess;

        if (instance != null)
            instance.Call("StartRegister", userName, passWord);
        else
        {
            this.rigisterSuccess("");
        }
    }

    void RegisterResult(string code)
    {
        Debug.Log("RegisterResult:" + code);
    }

    void NetworkUnavailable(string code)
    {
#if Release
        netWork.gameObject.SetActive(true);
        Invoke("Close", 2f);
#endif
    }

    void Close()
    {
        Application.Quit();
    }

    void RegisterSuccess(string vid)
    {
        Debug.Log("RegisterSuccess:" + vid);
        if (rigisterSuccess != null)
            rigisterSuccess(vid);
    }

    void Registering(string times)
    {
        if (rigistering != null)
            rigistering(times);

        string[] splitTimes = times.Split(':');
    }

    public void StartVerify(string userName, string passWord, Action verifySuccess, Action<string> verifyFailed)
    {
        this.verifySuccess = verifySuccess;
        this.verifyFailed = verifyFailed;

        if (instance != null)
            instance.Call("StartVerify", userName, passWord);
    }

    void VerifySuccess()
    {
        if (verifySuccess != null)
            verifySuccess();
    }

    void VerifyFailed(string error)
    {
        if (verifyFailed != null)
            verifyFailed(error);
    }
}
