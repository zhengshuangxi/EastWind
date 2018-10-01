using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class Conversation : Singleton<Conversation>
{
    const float WAIT_TIME = 2f;
    const int SUCCESS = 0;
    const int FAILED = -1;

    public Sprite[] vols;

    Transform stars;
    Transform waiter;
    Transform customer;
    Transform volume;

    Agent agent = null;
    Curriculum curriculum;

    Dialog dialog;
    int index = 0;
    int triggerId = 0;
    int answerIndex = 0;
    float totalScore = 0f;

    IPlay iPlay;

    bool voiceHint = false;
    float deltaTime = 0f;

    Transform cam;

    public void Awake()
    {
        stars = transform.Find("Panel/Stars");
        waiter = transform.Find("Panel/Waiter");
        customer = transform.Find("Panel/Customer");
        volume = transform.Find("Panel/Volume");
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        agent = GameObject.Find("Agent").GetComponent<Agent>();

         curriculum = Curriculum.Get(Loading.scene);
        //curriculum = Curriculum.Get("Dinner");

        ShowStar(0);

        DisplayHints(null);
    }

    private void Start()
    {
        waiter.Find("EN").GetComponent<Text>().text = "";
        waiter.Find("CN").GetComponent<Text>().text = "";

        customer.Find("EN").GetComponent<Text>().text = "";
        customer.Find("CN").GetComponent<Text>().text = "";
    }

    public void StartAct()
    {
        StartCoroutine(EvaluationProcess());
    }

    public void Register(IPlay iPlay)
    {
        this.iPlay = iPlay;
    }

    IEnumerator EvaluationProcess()
    {
        yield return StartCoroutine(iPlay.Begin());

        yield return StartCoroutine(Evaluating(0, SUCCESS));
    }

    IEnumerator Evaluating(float time, int code)
    {
        yield return new WaitForSeconds(time);

        if (code == SUCCESS)
        {
            yield return StartCoroutine(ShowDialog());
        }
        else if (code == FAILED)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ShowHint());
        }
    }

    void DisplayHints(List<int> answers)
    {
        if (answers == null)
        {
            customer.Find("Answers").gameObject.SetActive(false);
        }
        //else
        //{
        //    customer.Find("Answers").gameObject.SetActive(true);
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (i < answers.Count)
        //        {
        //            Sentence sen = Sentence.Get(answers[i]);
        //            customer.Find("Answers/" + i).GetComponent<Text>().text = sen.en;
        //        }
        //        else
        //            customer.Find("Answers/" + i).GetComponent<Text>().text = "";
        //    }
        //}
    }

    IEnumerator ShowDialog()
    {
        DisplayHints(null);

        customer.Find("EN").GetComponent<Text>().text = "";
        customer.Find("CN").GetComponent<Text>().text = "";

        Debug.Log("===" + index);
        if (index >= curriculum.dialogs.Count)//index is greater than dialog‘s count, finish
        {
            yield return new WaitForSeconds(0.5f);
            Finish();
            yield break;
        }

        dialog = curriculum.dialogs[index];

        if (dialog.triggerId != 0)//if triggerId is not zero, find the dialog's triggerId equals zero or equals the triggerId
        {
            for (; index < curriculum.dialogs.Count; index++)
            {
                if ((curriculum.dialogs[index].triggerId != 0 && curriculum.dialogs[index].triggerId == triggerId) || (curriculum.dialogs[index].triggerId == 0))
                {
                    triggerId = 0;
                    break;
                }
            }
        }

        dialog = curriculum.dialogs[index];//reset the dialog to correct one

        if (dialog.transfer == 1)
        {
            yield return new WaitForSeconds(4f);
            iPlay.Transfer(dialog.transfer);
            yield return new WaitForSeconds(1f);
        }

        if (dialog.askId >= 0)
        {
            waiter.Find("EN").GetComponent<Text>().text = Sentence.Get(dialog.askId).en;
            waiter.Find("CN").GetComponent<Text>().text = Role.currentRole.isReview ? "" : Sentence.Get(dialog.askId).cn;
            float waiterTime = Audio.GetInstance().Play(AudioType.INTRO, Sentence.Get(dialog.askId).audio);
            iPlay.PlayAnimation(Sentence.Get(dialog.askId).anim);
            yield return new WaitForSeconds(waiterTime);
            Role.AddToReviewByDialog(dialog.askId);
        }

        if (dialog.answerIds.Count == 1)//only ont answer
        {
            answerIndex = 0;

            DisplayAnswer(answerIndex);
        }
        else if (dialog.answerIds.Count > 1)//multy-answers
        {
            if (string.IsNullOrEmpty(dialog.keyword))//no keyword, random select one
            {
                answerIndex = UnityEngine.Random.Range(0, dialog.answerIds.Count);

                DisplayAnswer(answerIndex);
            }
            else//have keyword, display items according to dialog, and waiting for user voice input
            {
                iPlay.Process(dialog);
                //agent.StartRecognize(RecognizeResult);
                DisplayHints(dialog.answerIds);
                voiceHint = true;
                deltaTime = 0f;
            }
        }
        else//no customer dialog, finish
        {
            if (index == curriculum.dialogs.Count - 1)
            {
                yield return new WaitForSeconds(0.5f);
                Finish();
                yield return null;
            }
            else
            {
                index += 1;
                yield return StartCoroutine(ShowDialog());
            }
        }
    }

    IEnumerator ShowHint()
    {
        float hintTime = Audio.GetInstance().Play(AudioType.INTRO, Prompt.Get().Audio);
        yield return new WaitForSeconds(hintTime);
        DisplayAnswer(answerIndex);
    }


        void EvaluatorResult(string content)
    {
        voiceHint = false;
        Result result = XmlParser.Parse(content);

        if (result.error == Error.NORMAL)
        {
            Sentence sentence = Sentence.Get(dialog.answerIds[answerIndex]);

            Server.GetInstance().Studying(sentence, result.score.total);

            iPlay.PlayAnimation(sentence.anim);
            iPlay.Hide();

            totalScore += result.score.total;

            float time = 0.5f;

            if (result.score.total > 4)
            {
                Hint.GetInstance().Show("棒极了！", "Perfect!");
                //time = Audio.GetInstance().Play(AudioType.INTRO, "perfect");
            }
            else if (result.score.total > 3)
            {
                Hint.GetInstance().Show("非常棒！", "Great!");
                //time = Audio.GetInstance().Play(AudioType.INTRO, "great");
            }
            else if (result.score.total > 2)
            {
                Hint.GetInstance().Show("不错！", "Good!");
                //time = Audio.GetInstance().Play(AudioType.INTRO, "good");
            }
            else
            {
                Hint.GetInstance().Show("加油！", "Come on!");
                //time = Audio.GetInstance().Play(AudioType.INTRO, "comeon");
            }

            ShowStar(result.score.total);

            index++;
            StartCoroutine(Evaluating(time, SUCCESS));
        }
        else
        {
            StartCoroutine(Evaluating(0, FAILED));
        }
    }

    //void RecognizeResult(string content)
    //{
    //    voiceHint = false;

    //    content = content.TrimEnd('.');
    //    bool recognizeOK = false;

    //    for (int i = 0; i < dialog.answerIds.Count; i++)
    //    {
    //        Sentence sentence = Sentence.Get(dialog.answerIds[i]);
    //        if (sentence.en.ToUpper().Contains(content.ToUpper()))
    //        {
    //            Debug.Log(sentence.en + " contains " + content);
    //            triggerId = dialog.answerIds[i];
    //            answerIndex = i;

    //            iPlay.Select(dialog.answerIds[i]);

    //            if (dialog.transfer == 2)
    //            {
    //                index++;
    //                StartCoroutine(Evaluating(0, SUCCESS));
    //                recognizeOK = true;
    //                break;
    //            }
    //            else
    //            {
    //                DisplayAnswer(answerIndex);
    //                recognizeOK = true;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log(sentence.en + " not contains " + content);
    //        }
    //    }

    //    if (recognizeOK == false)
    //    {
    //        StartCoroutine(ShowPrompt());
    //    }
    //}

    //IEnumerator ShowPrompt()
    //{
    //    yield return new WaitForSeconds(1f);

    //    float time = Audio.GetInstance().Play(AudioType.INTRO, Prompt.Get().Audio);
    //    yield return new WaitForSeconds(time);

    //    agent.StartRecognize(RecognizeResult);
    //    voiceHint = true;
    //    deltaTime = 0f;
    //}

    void DisplayAnswer(int aIndex)
    {
        Debug.Log("DisplayAnswer:" + aIndex);
        iPlay.Display(Sentence.Get(dialog.answerIds[aIndex]).itemName);
        customer.Find("EN").GetComponent<Text>().text = Sentence.Get(dialog.answerIds[aIndex]).en;
        customer.Find("CN").GetComponent<Text>().text = Role.currentRole.isReview ? "" : Sentence.Get(dialog.answerIds[aIndex]).cn;
        agent.StartEvaluator(EvaluatorResult, Sentence.Get(dialog.answerIds[aIndex]).en);

        ///测试
        //Hint.GetInstance().Show("棒极了！", "Perfect!");
        //ShowStar(5);
        //index++;
        //StartCoroutine(Evaluating(0.5f, SUCCESS));


        voiceHint = true;
        deltaTime = 0f;
        Role.AddToReviewByDialog(dialog.answerIds[aIndex]);
    }

    void ShowStar(float score)
    {
        int sc = (int)score * 20 + 10;
        for (int i = 1; i <= 5; i++)
        {
            stars.Find(i + "/Full").gameObject.SetActive(false);
            stars.Find(i + "/Half").gameObject.SetActive(false);
            stars.Find(i + "/No").gameObject.SetActive(false);

            if (sc >= i * 20)
            {
                stars.Find(i + "/Full").gameObject.SetActive(true);
            }
            else
            {
                stars.Find(i + "/Full").gameObject.SetActive(false);
                if (sc >= (i - 1) * 20 + 10)
                    stars.Find(i + "/Half").gameObject.SetActive(true);
                else
                {
                    stars.Find(i + "/Half").gameObject.SetActive(false);
                    stars.Find(i + "/No").gameObject.SetActive(true);
                }
            }
        }
    }

    void Finish()
    {
        Debug.Log("Finish");
        transform.gameObject.SetActive(false);

        iPlay.Finish(totalScore);

        voiceHint = false;
    }

    private void Update()
    {
        if (voiceHint == false)
        {
            volume.GetComponent<Image>().sprite = vols[0];
        }
        else
        {
            deltaTime += Time.deltaTime;
            volume.GetComponent<Image>().sprite = vols[((int)(deltaTime / 0.2f)) % 3];
        }

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                for (int i = 0; i < dialog.answerIds.Count; i++)
                {
                    Sentence sentence = Sentence.Get(dialog.answerIds[i]);
                    if (sentence.en.ToUpper().Contains(hit.transform.name.ToUpper()))
                    {
                        Debug.Log(sentence.en + " contains " + hit.transform.name);
                        triggerId = dialog.answerIds[i];
                        answerIndex = i;

                        iPlay.Select(dialog.answerIds[i]);

                        if (dialog.transfer == 2)
                        {
                            index++;
                            StartCoroutine(Evaluating(0, SUCCESS));
                            break;
                        }
                        else
                        {
                            DisplayAnswer(answerIndex);
                            break;
                        }
                    }
                }
            }
        }
    }
}
