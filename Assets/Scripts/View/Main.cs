using UnityEngine;
using UnityEngine.EventSystems;
using Util.UI;

public class Main : EventTriggerListener
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
            case "Courses":
                Courses();
                break;
            case "Review":
                Review();
                break;
            case "Ability":
                Ability();
                break;
            case "Achivement":
                Achivement();
                break;
            case "Back":
                Role.Exit();
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void Courses()
    {
        UIManager<Course>.Show(transform.gameObject, true);
    }

    public void Review()
    {
        UIManager<Main>.Hide();

        ReviewGraph.GetInstance().Show();
    }

    public void Ability()
    {
        UIManager<Ability>.Show(transform.gameObject, true);
    }

    public void Achivement()
    {
        UIManager<Achivement>.Show(transform.gameObject, true);
    }
}
