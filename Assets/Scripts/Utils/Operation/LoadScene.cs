using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Operation;

public class LoadScene : IOperation
{
    string sceneName;
    AsyncOperation async;

    public LoadScene(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public IOperation Start()
    {
        async=SceneManager.LoadSceneAsync(sceneName);
        return this;
    }


    public bool IsFinished()
    {
        return async.isDone;
    }

    public void Click(string item)
    {

    }
}