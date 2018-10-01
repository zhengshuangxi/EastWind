using System.Collections.Generic;
using UnityEngine.EventSystems;
using Util.UI;

public class Ability : EventTriggerListener
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
            case "EnglishAbility":
                EnglishAbility();
                break;
            case "KnowledgeGraph":
                KnowledgeGraph();
                break;
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    public void EnglishAbility()
    {

    }

    public void KnowledgeGraph()
    {
        UIManager<Ability>.Hide(true);
        List<Point> pointList = new List<Point>();

        Dictionary<int, Point>.Enumerator iter = Point.points.GetEnumerator();
        while (iter.MoveNext())
        {
            pointList.Add(iter.Current.Value);
        }
        KonwledgeGraph.GetInstance().Show(pointList);
    }

    public void Back()
    {
        UIManager<Ability>.Hide();
    }
}
