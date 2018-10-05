using System;
using System.Collections;
using System.Collections.Generic;
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
    Agent agent = null;
    float interval = 1f;
    List<Transform> answers = new List<Transform>();

    void Awake()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        agent = GameObject.Find("Agent").GetComponent<Agent>();
        question.GetComponent<Question>().SetCallBack(ClickCallBack);

        for (int i = 1; i <= 5; i++)
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

        waiter.GetComponent<Animation>().Play(dialogue.anim);
        float time = Audio.GetInstance().Play(AudioType.INTRO, dialogue.audio);
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = dialogue.waiterText;
        dialogueWaiter.gameObject.SetActive(true);
        if (dialogue.displayDragonText)
        {
            dialogueDragon.Find("Text").GetComponent<TextMesh>().text = dialogue.dragonText;
            dialogueDragon.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        ShowQuestion(dialogue.answerContents);
    }

    IEnumerator DialogueOne()
    {
        successCallBack = DialogueTwo;

        waiter.GetComponent<Animation>().Play("Ask_1");
        float time = Audio.GetInstance().Play(AudioType.INTRO, "Restaurant/doforyou");
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = "What can I do for you, sir?";
        dialogueWaiter.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        dialogueGuest.Find("Text").GetComponent<TextMesh>().text = "May I have a menu, please?";
        dialogueGuest.gameObject.SetActive(true);

        dialogueDragon.Find("Text").GetComponent<TextMesh>().text = "menu n.菜单 美 [ˈmɛnju, ˈmenju]";
        dialogueDragon.gameObject.SetActive(true);
        agent.StartEvaluator(ReceiveEvaluatorResult, "May I have a menu, please?");
    }

    IEnumerator DialogueTwo()
    {
        Debug.LogError("DialogueTwo()");
        yield return StartCoroutine(DialogueDisplay(Dialogue.dialogueTwo));
    }

    IEnumerator DialogueSeven()
    {
        dialogueWaiter.gameObject.SetActive(false);
        dialogueGuest.gameObject.SetActive(false);
        dialogueDragon.gameObject.SetActive(false);
        ShowQuestion(new List<string>());

        yield return new WaitForSeconds(interval);

        waiter.GetComponent<Animation>().Play("OK");
        float time = Audio.GetInstance().Play(AudioType.INTRO, "Restaurant/enjoy");
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = "Hope you enjoy your meal!";
        dialogueWaiter.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        Loading.scene = "Main";
        SceneManager.LoadScene("Loading");
    }

    IEnumerator StartCallBack(CallBack callBack)
    {
        yield return StartCoroutine(callBack());
    }

    void ReceiveEvaluatorResult(string content)
    {
        Debug.LogError("EvaluatorResult");

        Result result = XmlParser.Parse(content);

        if (result.error == Error.NORMAL)
        {
            Debug.LogError("EvaluatorResult Error.NORMAL");
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
            }

        }
        else
        {
            Debug.LogError("EvaluatorResult Error.Other");
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

    public void ClickCallBack(string content)
    {
        if (content == "A.Salad")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSalad));
        }
        else if (content == "B.Soup")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSoup));
        }
        else if (content == "A.Caesar Salad"
            || content == "B.Mixed Vegetables Salad"
            || content == "C.Seafood Salad With Fruit"
            || content == "D.Tuna Fish Salad"
            || content == "E.Smoked Salmon Salad")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSaladDressing));
        }
        else if (content == "A.Steak")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSteak));
        }
        else if (content == "A.Rare"
            || content == "B.Medium"
            || content == "C.Well-done")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSauce));
        }
        else if (content == "B.Pasta")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialoguePasta));
        }
        else if (content == "C.Hamburger")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueHamburger));
        }
        else if (content == "A.Coffee, Milk, Tea, Water")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueCoffe));
        }
        else if (content == "B.Juice")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueJuice));
        }
        else if (content == "A.Caesar"
            || content == "B.Thousand Island"
            || content == "C.Vinaigrette"
            || content == "D.Ranch"
            || content == "A.Cream Mushroom Soup"
            || content == "B.Traditional Tomato Soup"
            || content == "C.French Onion Soup"
            || content == "D.Borsch"
            )
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueThree));
        }
        else if (content == "A.Black Pepper Sauce"
            || content == "B.Red Wine Sauce"
            || content == "C.Creamy Mushroom Sauce"
            || content == "A.Spaghetti"
            || content == "B.Bow Ties"
            || content == "C.Shells"
            || content == "D.Spirals"
            || content == "E.Ravioli"
            || content == "A.Chicken"
            || content == "B.Pork"
            || content == "C.Beef"
            || content == "D.Shrimp"
            || content == "E.Bacon"
            )
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueFour));
        }
        else if (content == "A.Ice Cream"
            || content == "B.Cake"
            || content == "C.Chocolate"
            || content == "D.Cookies"
            || content == "E.Pudding")
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueFive));
        }
        else if (content == "C.Coco cola, Wine"
            || content == "A.Hot, please."
            || content == "B.Cold, please."
            || content == "A.Applce"
            || content == "B.Orange"
            || content == "C.Grape"
            || content == "D.Pine Apple"
            || content == "E.Kiwi"
            )
        {
            StartCoroutine(DialogueDisplay(Dialogue.dialogueSix));
        }
        else if (content == "A.No, thanks!"
            || content == "B.Maybe later.")
        {
            StartCoroutine(StartCallBack(DialogueSeven));
        }
    }
}
