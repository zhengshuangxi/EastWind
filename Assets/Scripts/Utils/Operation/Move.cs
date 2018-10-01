using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Operation;

public class Move : IOperation
{
    Transform trans;
    Vector3 dst;
    float time;
    Action finish;

    float elapseTime = 0;
    Vector3 dir;
    Vector3 scale;

    public Move(Transform trans, Vector3 dst, float time, Action finish)
    {
        this.trans = trans;
        this.dst = dst;
        this.time = time;
        this.finish = finish;

        dir = (dst - trans.localPosition) / time;
    }

    public IOperation Start()
    {
        return this;
    }


    public bool IsFinished()
    {
        elapseTime += Time.deltaTime;
        if (elapseTime < time)
        {
            trans.localPosition += dir * Time.deltaTime;
        }
        else
        {
            trans.localPosition = dst;
            if (finish != null)
                finish();
        }

        return elapseTime >= time;
    }

    public void Click(string item)
    {

    }
}