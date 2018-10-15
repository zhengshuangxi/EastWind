using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Util.UI;

public class Record : EventTriggerListener
{
    Transform newRecord;
    Transform congratulations;


    public override void Awake()
    {
        base.Awake();

        //newRecord = transform.Find("Panel/Record");
        //congratulations = transform.Find("Congratulations");

        //newRecord.gameObject.SetActive(false);
        //congratulations.gameObject.SetActive(false);
    }

    public void Display(float score)
    {
        Transform star = transform.Find("Panel/Star");

        for (int j = 1; j < 5; j++)
        {
            star.Find(j.ToString()).gameObject.SetActive(false);
        }

        int intScore = (int)score;
        float floatScore = score - intScore;

        transform.gameObject.SetActive(true);


        int i = 1;

        for (; i <= intScore; i++)
        {
            Transform starTrans = star.Find(i.ToString());
            starTrans.gameObject.SetActive(true);
            starTrans.Find("Full").gameObject.SetActive(true);
            starTrans.Find("Half").gameObject.SetActive(false);
            starTrans.Find("No").gameObject.SetActive(false);
        }

        if (floatScore > 0)
        {
            Transform starTrans2 = star.Find(i.ToString());
            starTrans2.gameObject.SetActive(true);
            starTrans2.Find("Full").gameObject.SetActive(false);
            starTrans2.Find("Half").gameObject.SetActive(true);
            starTrans2.Find("No").gameObject.SetActive(false);
        }
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    //public void Show(bool isNewRecord)
    //{
    //    congratulations.gameObject.SetActive(true);
    //    newRecord.gameObject.SetActive(isNewRecord);
    //}

    public void Back()
    {
        //Server.GetInstance().FinishStudy();

        UIManager<Record>.Hide();

        Loading.scene = "Main";
        SceneManager.LoadScene("Loading");
    }
}
