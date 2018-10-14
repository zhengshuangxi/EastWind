using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Util.UI;

public class Course : EventTriggerListener
{
    public override void Awake()
    {
        base.Awake();

#if Release
        transform.Find("Panel/Police").gameObject.SetActive(false);
#endif
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Restaurant":
                Restaurant();
                break;
            case "Market":
                ShoppingMall();
                break;
            case "Police":
                PoliceOffice();
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
        if (go.transform.parent.name == "Restaurant")
        {
            Restaurant();
        }
        else if (go.transform.parent.name == "Shopping Mall")
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

    public static void PoliceOffice()
    {
        //Loading.scene = "Stroll";
        //SceneManager.LoadScene("Loading");
        //Server.GetInstance().StartStudy(6);
        Loading.scene = "PoliceOffice";
        SceneManager.LoadScene("Loading");
    }


    public void Back()
    {
        UIManager<Course>.Hide();
    }
}
