using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        for (int i = 1; i <= 5; i++)
        {
            Transform answer = question.Find("Answer" + i);
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
        agent.StartEvaluator(EvaluatorResult, "May I have a menu, please?");
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

        waiter.GetComponent<Animation>().Play("OK");
        float time = Audio.GetInstance().Play(AudioType.INTRO, "Restaurant/enjoy");
        dialogueWaiter.Find("Text").GetComponent<TextMesh>().text = "Hope you enjoy your meal!";
        dialogueWaiter.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        Loading.scene = "Stroll";
        SceneManager.LoadScene("Loading");
    }

    IEnumerator StartCallBack(CallBack callBack)
    {
        yield return StartCoroutine(callBack());
    }

    void EvaluatorResult(string content)
    {
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
            }
        }
        else
        {
            agent.StartEvaluator(EvaluatorResult, content);
        }
    }

    void ShowQuestion(List<string> answerContents)
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if (i < answerContents.Count)
            {
                answers[i].gameObject.SetActive(true);
                answers[i].Find("Text").GetComponent<TextMesh>().text = answerContents[i];
            }
            else
            {
                answers[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Transform text = hit.transform.Find("Text");
                if (text != null)
                {
                    if (text.GetComponent<TextMesh>().text == "A.Salad")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSalad));
                    }
                    else if (text.GetComponent<TextMesh>().text == "B.Soup")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSoup));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Caesar Salad"
                        || text.GetComponent<TextMesh>().text == "B.Mixed Vegetables Salad"
                        || text.GetComponent<TextMesh>().text == "C.Seafood Salad With Fruit"
                        || text.GetComponent<TextMesh>().text == "D.Tuna Fish Salad"
                        || text.GetComponent<TextMesh>().text == "E.Smoked Salmon Salad")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSaladDressing));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Steak")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSteak));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Rare"
                        || text.GetComponent<TextMesh>().text == "B.Medium"
                        || text.GetComponent<TextMesh>().text == "C.Well-done")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSauce));
                    }
                    else if (text.GetComponent<TextMesh>().text == "B.Pasta")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialoguePasta));
                    }
                    else if (text.GetComponent<TextMesh>().text == "C.Hamburger")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueHamburger));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Coffee, Milk, Tea, Water")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueCoffe));
                    }
                    else if (text.GetComponent<TextMesh>().text == "B.Juice")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueJuice));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Caesar"
                        || text.GetComponent<TextMesh>().text == "B.Thousand Island"
                        || text.GetComponent<TextMesh>().text == "C.Vinaigrette"
                        || text.GetComponent<TextMesh>().text == "D.Ranch"
                        || text.GetComponent<TextMesh>().text == "A.Cream Mushroom Soup"
                        || text.GetComponent<TextMesh>().text == "B.Traditional Tomato Soup"
                        || text.GetComponent<TextMesh>().text == "C.French Onion Soup"
                        || text.GetComponent<TextMesh>().text == "D.Borsch"
                        )
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueThree));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Black Pepper Sauce"
                        || text.GetComponent<TextMesh>().text == "B.Red Wine Sauce"
                        || text.GetComponent<TextMesh>().text == "C.Creamy Mushroom Sauce"
                        || text.GetComponent<TextMesh>().text == "A.Spaghetti"
                        || text.GetComponent<TextMesh>().text == "B.Bow Ties"
                        || text.GetComponent<TextMesh>().text == "C.Shells"
                        || text.GetComponent<TextMesh>().text == "D.Spirals"
                        || text.GetComponent<TextMesh>().text == "E.Ravioli"
                        || text.GetComponent<TextMesh>().text == "A.Chicken"
                        || text.GetComponent<TextMesh>().text == "B.Pork"
                        || text.GetComponent<TextMesh>().text == "C.Beef"
                        || text.GetComponent<TextMesh>().text == "D.Shrimp"
                        || text.GetComponent<TextMesh>().text == "E.Bacon"
                        )
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueFour));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.Ice Cream"
                        || text.GetComponent<TextMesh>().text == "B.Cake"
                        || text.GetComponent<TextMesh>().text == "C.Chocolate"
                        || text.GetComponent<TextMesh>().text == "D.Cookies"
                        || text.GetComponent<TextMesh>().text == "E.Pudding")
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueFive));
                    }
                    else if (text.GetComponent<TextMesh>().text == "C.Coco cola, Wine"
                        || text.GetComponent<TextMesh>().text == "A.Hot, please."
                        || text.GetComponent<TextMesh>().text == "B.Cold, please."
                        || text.GetComponent<TextMesh>().text == "A.Applce"
                        || text.GetComponent<TextMesh>().text == "B.Orange"
                        || text.GetComponent<TextMesh>().text == "C.Grape"
                        || text.GetComponent<TextMesh>().text == "D.Pine Apple"
                        || text.GetComponent<TextMesh>().text == "E.Kiwi"
                        )
                    {
                        StartCoroutine(DialogueDisplay(Dialogue.dialogueSix));
                    }
                    else if (text.GetComponent<TextMesh>().text == "A.No, thanks!"
                        || text.GetComponent<TextMesh>().text == "B.Maybe later.")
                    {
                        StartCoroutine(StartCallBack(DialogueSeven));
                    }
                }
            }
        }
    }
}
