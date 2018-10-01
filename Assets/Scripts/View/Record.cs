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

        newRecord = transform.Find("Panel/Record");
        congratulations = transform.Find("Congratulations");

        newRecord.gameObject.SetActive(false);
        congratulations.gameObject.SetActive(false);
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

    public void Show(bool isNewRecord)
    {
        congratulations.gameObject.SetActive(true);
        newRecord.gameObject.SetActive(isNewRecord);
    }

    public void Back()
    {
        Server.GetInstance().FinishStudy();

        UIManager<Record>.Hide();

        Loading.scene = "Main";
        SceneManager.LoadScene("Loading");
    }
}
