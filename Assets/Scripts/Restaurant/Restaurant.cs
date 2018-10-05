using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restaurant : MonoBehaviour
{
    delegate IEnumerator CallBack();

    public Transform waiter;
    public Transform dragon;
    public Transform dialogueWaiter;
    public Transform dialogueGuest;
    public Transform dialogueDragon;
    public Transform question;

    Transform cam;
    CallBack successCallBack = null;
    bool repeatEvaluate = false;
    Agent agent = null;
    float interval = 1f;
    List<Transform> answers = new List<Transform>();
    string prefix = "I would like ";
    string regularExpression = "[A-Z]+";

    void Awake()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        agent = GameObject.Find("Agent").GetComponent<Agent>();
        question.GetComponent<Question>().SetCallBack(ClickCallBack);

        for (int i = 1; i <= 7; i++)
        {
            Transform answer = question.Find("Panel/Answer" + i);
            answer.gameObject.SetActive(false);
            answers.Add(answer);
        }

        dialogueWaiter.gameObject.SetActive(false);
        dialogueGuest.gameObject.SetActive(false);
        dialogueDragon.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(DialogueOne());
    }

    IEnumerator DialogueDisplay(Dialogue dialogue)
    {
        dialogueWaiter.gameObject.SetActive(false);
        dialogueGuest.gameObject.SetActive(false);
        dialogueDragon.gameObject.SetActive(false);
        ShowQuestion(new List<string>());

        yield return new WaitForSeconds(interval);

        float time = Audio.GetInstance().Play(AudioType.INTRO, dialogue.audio);
        PlayAnimation(dialogue.anim, time);
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = dialogue.waiterText;
        dialogueWaiter.gameObject.SetActive(true);
        if (dialogue.displayDragonText)
        {
            dialogueDragon.Find("Text").GetComponent<TextMesh>().text = dialogue.dragonText;
            dialogueDragon.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        StopAnimation(dialogue.anim);
        ShowQuestion(dialogue.answerContents);
    }

    void PlayAnimation(string anim, float time)
    {
        Animation animation = waiter.GetComponent<Animation>();
        animation[anim].speed = animation[anim].length / time;
        animation.Play(anim);
    }

    void StopAnimation(string anim)
    {
        waiter.GetComponent<Animation>().Stop(anim);
    }

    IEnumerator DialogueOne()
    {
        successCallBack = DialogueTwo;
        repeatEvaluate = true;

        float time = Audio.GetInstance().Play(AudioType.INTRO, "Restaurant/doforyou");
        PlayAnimation("Ask_1", time);
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = "What can I do for you, sir?";
        dialogueWaiter.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        StopAnimation("Ask_1");

        dialogueGuest.Find("Text").GetComponent<TextMesh>().text = "May I have a menu, please?";
        dialogueGuest.gameObject.SetActive(true);

        dialogueDragon.Find("Text").GetComponent<TextMesh>().text = "menu n.菜单 美 [ˈmɛnju, ˈmenju]";
        dialogueDragon.gameObject.SetActive(true);
        dialogueGuest.Find("InputHint").GetComponent<InputHint>().StartShow();
        agent.StartEvaluator(ReceiveEvaluatorResult, "May I have a menu, please?");
    }

    IEnumerator DialogueTwo()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueTwo));
    }

    IEnumerator DialogueSeven()
    {
        dialogueWaiter.gameObject.SetActive(false);
        dialogueGuest.gameObject.SetActive(false);
        dialogueDragon.gameObject.SetActive(false);
        ShowQuestion(new List<string>());

        yield return new WaitForSeconds(interval);

        float time = Audio.GetInstance().Play(AudioType.INTRO, "Restaurant/enjoy");
        PlayAnimation("OK", time);
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = "Hope you enjoy your meal!";
        dialogueWaiter.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        StopAnimation("OK");

        Loading.scene = "Main";
        SceneManager.LoadScene("Loading");
    }

    IEnumerator StartCallBack(CallBack callBack)
    {
        yield return StartCoroutine(callBack());
    }

    void ReceiveEvaluatorResult(string content)
    {
        dialogueGuest.Find("InputHint").GetComponent<InputHint>().StopShow();

        Result result = XmlParser.Parse(content);

        if (result.error == Error.NORMAL)
        {
            float time = 0.5f;

            if (result.score.total > 4)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "perfect");
            }
            else if (result.score.total > 3)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "great");
            }
            else if (result.score.total > 2)
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "good");
            }
            else
            {
                time = Audio.GetInstance().Play(AudioType.INTRO, "comeon");
            }

            if (successCallBack != null)
            {
                StartCoroutine(StartCallBack(successCallBack));
                successCallBack = null;
            }
        }
        else if (repeatEvaluate)
        {
            agent.StartEvaluator(ReceiveEvaluatorResult, content);
        }
    }

    void ShowQuestion(List<string> answerContents)
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if (i < answerContents.Count)
            {
                answers[i].gameObject.SetActive(true);
                answers[i].Find("Text").GetComponent<Text>().text = answerContents[i];
            }
            else
            {
                answers[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DialogueSalad()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSalad));
    }

    IEnumerator DialogueSoup()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSoup));
    }

    IEnumerator DialogueSaladDressing()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSaladDressing));
    }

    IEnumerator DialogueSteak()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSteak));
    }

    IEnumerator DialogueSauce()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSauce));
    }

    IEnumerator DialoguePasta()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialoguePasta));
    }

    IEnumerator DialogueHamburger()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueHamburger));
    }

    IEnumerator DialogueCoffe()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueCoffe));
    }

    IEnumerator DialogueJuice()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueJuice));
    }

    IEnumerator DialogueThree()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueThree));
    }

    IEnumerator DialogueFour()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueFour));
    }

    IEnumerator DialogueFive()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueFive));
    }

    IEnumerator DialogueSix()
    {
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueSix));
    }

    public void ClickCallBack(string content)
    {
        if (content == "salad")
        {
            successCallBack = DialogueSalad;
        }
        else if (content == "soup")
        {
            successCallBack = DialogueSoup;
        }
        else if (content == "steak")
        {
            successCallBack = DialogueSteak;
        }
        else if (Dialogue.dialogueSalad.answerContents.Contains(content))
        {
            successCallBack = DialogueSaladDressing;
        }
        else if (Dialogue.dialogueSteak.answerContents.Contains(content))
        {
            successCallBack = DialogueSauce;
        }
        else if (content == "pasta")
        {
            successCallBack = DialoguePasta;
        }
        else if (content == "hamburger")
        {
            successCallBack = DialogueHamburger;
        }
        else if (content == "coffee" || content == "milk" || content == "tea" || content == "water")
        {
            successCallBack = DialogueCoffe;
        }
        else if (content == "juice")
        {
            successCallBack = DialogueJuice;
        }
        else if (Dialogue.dialogueSaladDressing.answerContents.Contains(content)
            || Dialogue.dialogueSoup.answerContents.Contains(content)
            )
        {
            successCallBack = DialogueThree;
        }
        else if (Dialogue.dialogueSauce.answerContents.Contains(content)
            || Dialogue.dialoguePasta.answerContents.Contains(content)
            || Dialogue.dialogueHamburger.answerContents.Contains(content)
            )
        {
            successCallBack = DialogueFour;
        }
        else if (Dialogue.dialogueFour.answerContents.Contains(content))
        {
            successCallBack = DialogueFive;
        }
        else if (content == "coco cola" || content == "wine"
            || Dialogue.dialogueCoffe.answerContents.Contains(content)
            || Dialogue.dialogueJuice.answerContents.Contains(content)
            )
        {
            successCallBack = DialogueSix;
        }
        else if (content == "No, thanks!" || content == "Maybe later.")
        {
            successCallBack = DialogueSeven;
        }

        repeatEvaluate = false;
        string evaContent = Regex.IsMatch(content, regularExpression) ? content : prefix + content;
        agent.StartEvaluator(ReceiveEvaluatorResult, evaContent);

        dialogueGuest.Find("Text").GetComponent<TextMesh>().text = evaContent;
        dialogueGuest.Find("InputHint").GetComponent<InputHint>().StartShow();
        dialogueGuest.gameObject.SetActive(true);
    }
}
