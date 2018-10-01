using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.UI;

public class Detail : EventTriggerListener
{
    Point point;
    Text en;
    Text cn;
    Text desc;
    Text times;

    public override void Awake()
    {
        base.Awake();

        en = transform.Find("Panel/Content/EN").GetComponent<Text>();
        cn = transform.Find("Panel/Content/CN").GetComponent<Text>();
        desc = transform.Find("Panel/Content/Desc").GetComponent<Text>();
        times = transform.Find("Panel/Times/Num").GetComponent<Text>();
    }

    public override void Show(object mObject)
    {
        base.Show(mObject);

        point = (Point)mObject;
        en.text = point.en;
        cn.text = point.cn;
        desc.text = point.desc;
        if (Role.currentRole.reviews.ContainsKey(point.id))
            times.text = Role.currentRole.reviews[point.id].times.ToString();
        else
            times.text = "0";
        if (Role.currentRole.reviews.ContainsKey(point.id) && Role.currentRole.reviews[point.id].active == false)
        {
            transform.Find("Panel/Recover").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Panel/Recover").gameObject.SetActive(false);
        }
    }

    public override void OnClick()
    {
        base.OnClick();
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Recover":
                Recover();
                break;
            case "Voice":
                Voice();
                break;
            case "Back":
                Back();
                break;
            default:
                break;
        }
    }

    void Recover()
    {
        Role.currentRole.reviews[point.id].active = true;
        transform.Find("Panel/Recover").gameObject.SetActive(false);
    }

    void Voice()
    {
        Audio.GetInstance().Play(AudioType.INTRO, point.audio);
    }

    void Back()
    {
        UIManager<Detail>.Hide();

        List<Point> pointList = new List<Point>();
        Dictionary<int, Point>.Enumerator iter = Point.points.GetEnumerator();
        while (iter.MoveNext())
        {
            pointList.Add(iter.Current.Value);
        }
        KonwledgeGraph.GetInstance().Show(pointList);
    }
}
