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
            default:
                break;
        }
    }

    public void Preview(GameObject go)
    {
        Role.currentRole.isReview = false;
        if(go.transform.parent.name== "Restaurant")
        {
            Restaurant();
        }
        else if(go.transform.parent.name == "Shopping Mall")
        {
            ShoppingMall();
        }
    }

    public void Review(GameObject go)
    {
        Role.currentRole.isReview = true;

        if (go.transform.parent.name == "Restaurant")
        {
            Restaurant();
            
        }
        else if (go.transform.parent.name == "Shopping Mall")
        {
            ShoppingMall();
        }
    }

    public static void Restaurant()
    {
        Loading.scene = "Restaurant";
        SceneManager.LoadScene("Loading");
    }

    public static void ShoppingMall()
    {
        Loading.scene = "Market";
        SceneManager.LoadScene("Loading");
    }

    public static void Stroll()
    {
        //Loading.scene = "Stroll";
        //SceneManager.LoadScene("Loading");
        //Server.GetInstance().StartStudy(6);
    }


    public void Back()
    {
        UIManager<Course>.Hide();
    }
}
