using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Util.UI;

public class Course : EventTriggerListener
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Preview":
                Preview(EventSystem.current.currentSelectedGameObject);
                break;
            case "Review":
                Review(EventSystem.current.currentSelectedGameObject);
                break;
            case "Back":
                Back();
                break;
            case "Test":
                Loading.scene = "Driver";
                SceneManager.LoadScene("Loading");
                break;
            default:
                break;
        }
    }

    public void Preview(GameObject go)
    {
        Role.currentRole.isReview = false;
        if(go.transform.parent.name== "Story")
        {
            Story();
            //Server.GetInstance().StartStudy(4);
        }
        else if(go.transform.parent.name == "Dinner")
        {
            Dinner();
            //Server.GetInstance().StartStudy(5);
        }
        else if (go.transform.parent.name == "Stroll")
        {
            Stroll();
           // Server.GetInstance().StartStudy(6);
        }
    }

    public void Review(GameObject go)
    {
        Role.currentRole.isReview = true;

        if (go.transform.parent.name == "Story")
        {
            Story();
            
        }
        else if (go.transform.parent.name == "Dinner")
        {
            Dinner();
            
        }
        else if (go.transform.parent.name == "Shopping")
        {
            Stroll();
            
        }
    }

    public static void Story()
    {
    }

    public static void Dinner()
    {
    }

    public static void Stroll()
    {
        Loading.scene = "Stroll";
        SceneManager.LoadScene("Loading");
        //Server.GetInstance().StartStudy(6);
    }


    public void Back()
    {
        UIManager<Course>.Hide();
    }
}
