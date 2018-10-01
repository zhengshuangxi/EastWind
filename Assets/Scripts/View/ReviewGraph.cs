using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.UI;

public class ReviewGraph : Singleton<ReviewGraph>
{
    const int MaxCount = 18;

    public Transform back;
    public Transform next;
    public Transform origin;
    public Transform brain;

    Agent agent = null;
    Transform cam;
    Transform focused;
    Vector3 focusedPos;
    bool show = false;

    int index = 0;

    Dictionary<Transform, Point> pointMap = new Dictionary<Transform, Point>();
    List<Point> pointList = new List<Point>();
    Stack<Transform> transStack = new Stack<Transform>();
    List<Vector3> posList = new List<Vector3>()
    {
        new Vector3(-1.5f,2.5f,6f),
        new Vector3(1.5f,2.5f,6f),
        new Vector3(3f,0f,6f),
        new Vector3(1.5f,-2.5f,6f),
        new Vector3(-1.5f,-2.5f,6f),
        new Vector3(-3f,0f,6f),
        new Vector3(-3f,5f,6f),
        new Vector3(0f,5f,6f),
        new Vector3(3f,5f,6f),
        new Vector3(4.5f,2.5f,6f),
        new Vector3(6f,0f,6f),
        new Vector3(4.5f,-2.5f,6f),
        new Vector3(3f,-5f,6f),
        new Vector3(0f,-5f,6f),
        new Vector3(-3f,-5f,6f),
        new Vector3(-4.5f,-2.5f,6f),
        new Vector3(-6f,0f,6f),
        new Vector3(-4.5f,2.5f,6f),
    };

    void Awake()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        agent = GameObject.Find("Agent").GetComponent<Agent>();

        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        origin.gameObject.SetActive(false);
        brain.gameObject.SetActive(false);
        brain.Find("Effects").gameObject.SetActive(false);

        ResetItem(origin);
    }

    void Hide()
    {
        show = false;
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        origin.gameObject.SetActive(false);

        Dictionary<Transform, Point>.Enumerator iter = pointMap.GetEnumerator();
        while (iter.MoveNext())
        {
            transStack.Push(iter.Current.Key);
            DisplayAnswer(iter.Current.Key, false);
            iter.Current.Key.gameObject.SetActive(false);
        }
    }

    void ResetItem(Transform trans)
    {
        DisplayAnswer(trans, false);
        trans.gameObject.SetActive(false);
    }

    void DisplayAnswer(Transform trans, bool disp)
    {
        trans.Find("Answer1").gameObject.SetActive(disp);
        trans.Find("Answer2").gameObject.SetActive(disp);
        trans.Find("Answer3").gameObject.SetActive(disp);
        trans.Find("Answer4").gameObject.SetActive(disp);
        trans.Find("Voice").gameObject.SetActive(disp);
        trans.Find("Remove").gameObject.SetActive(disp);
    }

    void SetItem(Transform trans, Point point)
    {
        List<string> confusions = Point.GetConfusionAnswers(point.id);

        trans.Find("Question/Text").GetComponent<TextMesh>().text = point.en;
        trans.Find("Question/Wrong").gameObject.SetActive(false);
        trans.Find("Question/Unselected").gameObject.SetActive(true);
        trans.Find("Question/Correct").gameObject.SetActive(false);

        int random = UnityEngine.Random.Range(1, 5);
        int index = 0;
        for (int i = 1; i <= 4; i++)
        {
            if (i == random)
            {
                trans.Find(string.Format("Answer{0}/Text", i)).GetComponent<TextMesh>().text = point.cn;
            }
            else
            {
                trans.Find(string.Format("Answer{0}/Text", i)).GetComponent<TextMesh>().text = confusions[index];
                index += 1;
            }
        }
    }

    public void Show()
    {
        pointList.Clear();

        Dictionary<int, ReviewRecord>.Enumerator iter = Role.currentRole.reviews.GetEnumerator();
        while (iter.MoveNext())
        {
            if (DateTime.Now.Ticks > iter.Current.Value.ticks && iter.Current.Value.active == true)
                pointList.Add(Point.points[iter.Current.Key]);
        }

        show = true;
        index = 0;
        int count = pointList.Count > MaxCount ? MaxCount : pointList.Count;
        ShowItems(count);
        next.gameObject.SetActive(pointList.Count > MaxCount);
        back.gameObject.SetActive(true);
    }

    void ShowItems(int count)
    {
        Dictionary<Transform, Point>.Enumerator iter = pointMap.GetEnumerator();
        while (iter.MoveNext())
        {
            ResetItem(iter.Current.Key);
            transStack.Push(iter.Current.Key);
        }
        pointMap.Clear();

        for (int i = 0; i < count; i++)
        {
            Transform trans = transStack.Count > 0 ? transStack.Pop() : GameObject.Instantiate(origin);
            trans.parent = transform;
            trans.localPosition = posList[i];
            trans.localScale = origin.localScale;
            SetItem(trans, pointList[index * MaxCount + i]);
            pointMap.Add(trans, pointList[index * MaxCount + i]);
            trans.gameObject.SetActive(true);
        }
    }

    void FocusItem(Transform trans)
    {
        if (focused != null)
        {
            focused.localPosition = focusedPos;
            DisplayAnswer(focused, false);
        }

        if (trans != null)
        {
            focused = trans;
            focusedPos = focused.localPosition;
            focused.localPosition = Vector3.zero;
            focused.Find("Question/Wrong").gameObject.SetActive(false);
            focused.Find("Question/Unselected").gameObject.SetActive(true);
            focused.Find("Question/Correct").gameObject.SetActive(false);

            DisplayAnswer(focused, true);

            back.gameObject.SetActive(false);
            next.gameObject.SetActive(false);
            brain.gameObject.SetActive(true);
        }
        else
        {
            back.gameObject.SetActive(true);
            next.gameObject.SetActive(pointList.Count > MaxCount);
            brain.gameObject.SetActive(false);
            brain.Find("Effects").gameObject.SetActive(false);
        }

        Dictionary<Transform, Point>.Enumerator iter = pointMap.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Key.gameObject.SetActive((trans != null && iter.Current.Key == trans) || (trans == null));
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
        Debug.LogError("ReviewGraph Back");
        if (index <= 0)
        {
            Hide();

            UIManager<Main>.Show();
        }
        else
        {
            index -= 1;
            int count = pointList.Count - index * MaxCount > MaxCount ? MaxCount : pointList.Count - index * MaxCount;
            ShowItems(count);
        }
    }

    void Voice(Point point)
    {
        agent.StartEvaluator(EvaluatorResult, point.en);
        Role.AddToReview(point.sentenceId);
    }

    void EvaluatorResult(string content)
    {
        Result result = XmlParser.Parse(content);

        if (result.error == Error.NORMAL)
        {
            float time;

            if (result.score.total > 4)
            {
                Hint.GetInstance().Show("棒极了！", "Perfect!");
                time = Audio.GetInstance().Play(AudioType.INTRO, "perfect");
            }
            else if (result.score.total > 3)
            {
                Hint.GetInstance().Show("非常棒！", "Great!");
                time = Audio.GetInstance().Play(AudioType.INTRO, "great");
            }
            else if (result.score.total > 2)
            {
                Hint.GetInstance().Show("不错！", "Good!");
                time = Audio.GetInstance().Play(AudioType.INTRO, "good");
            }
            else
            {
                Hint.GetInstance().Show("加油！", "Come on!");
                time = Audio.GetInstance().Play(AudioType.INTRO, "comeon");
            }
        }
    }

    void Remove(Point point)
    {
        if(Role.currentRole.reviews.ContainsKey(point.id))
        {
            FocusItem(null);
            Role.currentRole.reviews[point.id].active = false;
            Show();
        }
    }

    void Update()
    {
        if (show == false)
            return;

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.JoystickButton0)|| Input.GetMouseButtonDown(0))
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
                else if (pointMap.ContainsKey(hit.transform))
                {
                    FocusItem(hit.transform);
                }
                else if (pointMap.ContainsKey(hit.transform.parent))
                {
                    if (hit.transform.name == "Voice" || hit.transform.name == "Remove")
                    {
                        if (hit.transform.name == "Voice")
                        {
                            Voice(pointMap[hit.transform.parent]);
                        }
                        else
                        {
                            Remove(pointMap[hit.transform.parent]);
                        }
                    }
                    else
                    {
                        if(hit.transform.Find("Text").GetComponent<TextMesh>().text==pointMap[hit.transform.parent].cn)
                        {
                            hit.transform.parent.Find("Question/Wrong").gameObject.SetActive(false);
                            hit.transform.parent.Find("Question/Unselected").gameObject.SetActive(false);
                            hit.transform.parent.Find("Question/Correct").gameObject.SetActive(true);
                            Audio.GetInstance().Play(AudioType.OPER, "Correct");
                        }
                        else
                        {
                            hit.transform.parent.Find("Question/Wrong").gameObject.SetActive(true);
                            hit.transform.parent.Find("Question/Unselected").gameObject.SetActive(false);
                            hit.transform.parent.Find("Question/Correct").gameObject.SetActive(false);
                            Audio.GetInstance().Play(AudioType.OPER, "Wrong");
                        }
                    }
                }
            }
            else
            {
                FocusItem(null);
            }
        }
    }
}
