using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.UI;

public class KonwledgeGraph : Singleton<KonwledgeGraph>
{
    const int MaxCount = 16;

    public Transform back;
    public Transform next;
    public Transform origin;

    Transform cam;
    int index = 0;
    bool show = false;

    Dictionary<Transform, Point> konwledgeMap = new Dictionary<Transform, Point>();
    List<Point> pointList = new List<Point>();
    Stack<Transform> transStack = new Stack<Transform>();
    List<Vector3> posList = new List<Vector3>()
    {
        new Vector3(0.5f,0f,0f),
        new Vector3(0.25f,0.5f,0f),
        new Vector3(-0.25f,0.5f,0f),
        new Vector3(-0.5f,0f,0f),
        new Vector3(-0.25f,-0.5f,0f),
        new Vector3(0.25f,-0.5f,0f),
        new Vector3(1f,0f,0f),
        new Vector3(0.85f,0.7f,0f),
        new Vector3(0.3f,1f,0f),
        new Vector3(-0.3f,1f,0f),
        new Vector3(-0.85f,0.7f,0f),
        new Vector3(-1f,0f,0f),
        new Vector3(-0.85f,-0.7f,0f),
        new Vector3(-0.3f,-1f,0f),
        new Vector3(0.3f,-1f,0f),
        new Vector3(0.85f,-0.7f,0f),
    };

    void Awake()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;

        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        origin.gameObject.SetActive(false);
    }

    void Hide()
    {
        show = false;
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        origin.gameObject.SetActive(false);

        Dictionary<Transform, Point>.Enumerator iter = konwledgeMap.GetEnumerator();
        while (iter.MoveNext())
        {
            transStack.Push(iter.Current.Key);
            iter.Current.Key.gameObject.SetActive(false);
        }
    }

    public void Show(List<Point> pointList)
    {
        show = true;
        this.pointList = pointList;
        index = 0;
        int count = pointList.Count > MaxCount ? MaxCount : pointList.Count;
        ShowItems(count);
        next.gameObject.SetActive(pointList.Count > MaxCount);
        back.gameObject.SetActive(true);
    }

    void ShowItems(int count)
    {
        Dictionary<Transform, Point>.Enumerator iter = konwledgeMap.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Key.gameObject.SetActive(false);
            transStack.Push(iter.Current.Key);
        }
        konwledgeMap.Clear();

        for (int i = 0; i < count; i++)
        {
            Transform trans = transStack.Count > 0 ? transStack.Pop() : GameObject.Instantiate(origin);
            trans.parent = transform;
            trans.localPosition = posList[i];
            trans.localScale = origin.localScale;
            trans.Find("Text").GetComponent<TextMesh>().text = pointList[index * MaxCount + i].cn;
            konwledgeMap.Add(trans, pointList[index * MaxCount + i]);
            trans.gameObject.SetActive(true);
        }
    }

    void Next()
    {
        if (pointList.Count > index * MaxCount)
        {
            index += 1;
            int count = pointList.Count - index * MaxCount > MaxCount ? MaxCount : pointList.Count - index * MaxCount;
            ShowItems(count);
        }
    }

    void Back()
    {
        if (index <= 0)
        {
            Hide();
            UIManager<Ability>.Show();
        }
        else
        {
            index -= 1;
            int count = pointList.Count - index * MaxCount > MaxCount ? MaxCount : pointList.Count - index * MaxCount;
            ShowItems(count);
        }
    }

    void Update()
    {
        if (show == false)
            return;

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform == back)
                {
                    Back();
                }
                else if (hit.transform == next)
                {
                    Next();
                }
                else if (konwledgeMap.ContainsKey(hit.transform))
                {
                    Hide();
                    UIManager<Detail>.Show(konwledgeMap[hit.transform]);
                }
            }
        }
    }
}
