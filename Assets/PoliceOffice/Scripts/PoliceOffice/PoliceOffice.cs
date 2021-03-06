﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoliceOffice : MonoBehaviour {

    private Transform cam;
    private string currChoose;//当前选择的物品
    private List<string> goods = new List<string> { "Wallet", "Passport", "Bag" };
    public GameObject policeOffice_Prefab;
    public GameObject dialoguePolice;
    public GameObject dialogueOwner;
    private GameObject hintObj;//提示物体
    private GameObject hintSprite;//提示图片
    private Text en;//提示的内容
    DialogueUnit currDialogue;//当前对话
    private Queue<DialogueUnit> queDialogue = new Queue<DialogueUnit>();
    private Animation policeAnim;
    Agent agent = null;
    private Text policeText;
    private TextMesh ownerText;
    private GameObject dragon;

    public ParticleSystem thinkingCloud;
    //private GameObject bubble;
    public List<GameObject> bubbles;

    private float totalScore;
    private List<string> list;//出错单词收集
    public GameObject reload;
    public GameObject quit;
    public GameObject summarize;
    private Text scoreText;
    public GameObject clone;//显示错误点的item
    private Transform content;
    void Start()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        hintObj = GameObject.Find("Hint");
        hintSprite = Global.FindChild(hintObj.transform, "hint");
        en = Global.FindChild<Text>(hintObj.transform, "en");
        //hintObj.SetActive(false);
        policeAnim = Global.FindChild<Animation>(policeOffice_Prefab.transform, "Policeman_Animations");
        agent = GameObject.Find("Agent").GetComponent<Agent>();
        policeText = Global.FindChild<Text>(dialoguePolice.transform, "PoliceText");
        ownerText = Global.FindChild<TextMesh>(dialogueOwner.transform, "OwnerText");
        dragon = Global.FindChild(hintObj.transform, "Object002");
        scoreText = Global.FindChild<Text>(transform, "ScoreText");
        content = Global.FindChild<Transform>(summarize.transform, "Content");
        currChoose = PlayerPrefs.GetString("currChoose");

        //StartCoroutine(WaitForDosth(15, () =>
        //{
            
        //}));
        InitQueDialogue();
    }

    Ray ray;
    RaycastHit hit;
    float interval;//刷新开始时间
    bool isPrompt = false;
    void Update()
    {
        ray = new Ray(cam.position, cam.forward);
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                   if (hit.collider.gameObject == dragon)
                    {
                        isPrompt = !isPrompt;
                        hintSprite.SetActive(isPrompt);
                    }
                    else if (hit.collider.gameObject == reload)
                    {
                        SceneManager.LoadScene("PoliceStation");
                    }
                    else if (hit.collider.gameObject == quit)
                        Application.Quit();
                }
            }
        }

        if (queDialogue.Count > 0)
        {
            if (currDialogue != queDialogue.Peek())
            {
                //播放当前声音和动画
                currDialogue = queDialogue.Peek();
                if (currDialogue is OperateUnit)
                {
                    OperateUnit wallet = (OperateUnit)currDialogue;
                    interval = Audio.GetInstance().Play(AudioType.INTRO, wallet.AudioName);
                    policeAnim.Play(wallet.Anim);
                    policeText.text = wallet.Ask;
                    dialoguePolice.SetActive(true);
                    en.text = wallet.Hint;
                    StartCoroutine(WaitForDosth(interval, () =>
                    {
                        if (policeAnim.IsPlaying(wallet.Anim))
                            policeAnim.Stop();
                    }));
                }
            }
            else if (currDialogue == queDialogue.Peek())
            {
               
                if (!currDialogue.IsDone)
                {
                    //等待警察说话结束
                    interval -= Time.deltaTime;
                    if (interval <= 0)
                    {
#if Release
                        //开始刷新
                        currDialogue.Refresh(
                        (isHaveBubble, bubble) =>
                        {
                            policeAnim.Play("Write");
                            if (isHaveBubble)
                            {
                                //显示气泡
                                thinkingCloud.gameObject.SetActive(true);
                                thinkingCloud.Play();
                                GameObject bubbleObj = bubbles.Find(p => p.name.Equals(bubble));
                                bubbleObj.SetActive(true);
                            }
                        },
                        (args) =>
                        {
                            //当前完成
                            currDialogue.IsDone = true;
                            isPrompt = false;
                            //隐藏提示
                            if (hintSprite.activeSelf)
                                hintSprite.SetActive(false);
                            if (thinkingCloud.gameObject.activeSelf)
                            {
                                thinkingCloud.gameObject.SetActive(false);
                                GameObject bubbleObj = bubbles.Find(p => p.activeSelf);
                                bubbleObj.SetActive(false);
                            }
                            totalScore += (float)args[0];
                            list.AddRange((List<string>)args[1]);
                            //Debug.Log("总得分:" + Mathf.Round(totalScore));
                        });
#else
                        //按流程走
                        currDialogue.ByFlow(
                        (isHaveBubble, bubble) =>
                        {
                             policeAnim.Play("Write");
                            if (isHaveBubble)
                            {
                                //显示气泡
                                thinkingCloud.gameObject.SetActive(true);
                                thinkingCloud.Play();
                                GameObject bubbleObj = bubbles.Find(p => p.name.Equals(bubble));
                                bubbleObj.SetActive(true);
                            }
                        },
                        (args) =>
                        {
                              //当前完成
                            currDialogue.IsDone = true;
                            isPrompt = false;
                            //隐藏提示
                            if (hintSprite.activeSelf)
                                hintSprite.SetActive(false);
                            if (thinkingCloud.gameObject.activeSelf)
                            {
                                thinkingCloud.gameObject.SetActive(false);
                                GameObject bubbleObj = bubbles.Find(p => p.activeSelf);
                                bubbleObj.SetActive(false);
                            }
                            totalScore += (float)args[0];
                        }
                        );
#endif
                    }
                }
                else
                {
                    queDialogue.Dequeue();
                    currDialogue = null;
                }
            }
        }
        else if (queDialogue.Count == 0 && init)
        {
            scoreText.text = "Score:" + Mathf.Round(totalScore).ToString();
            if (!summarize.activeSelf)
                summarize.SetActive(true);
            if (!error)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GameObject obj = Instantiate(clone, content);
                    Text textClone = Global.FindChild<Text>(obj.transform, "Text");
                    textClone.text = list[i];
                }
                error = true;
            }
        }
    }

    private bool init;
    private bool error;
    //初始化对话队列
    private void InitQueDialogue()
    {
        totalScore = 0;//总分归0
        list = new List<string>();
        StartCoroutine(FileHelper.Load(FileHelper.filePath, (content) => {
            if (content.StartsWith(currChoose))
            {
                DialogueUnit unit = new OperateUnit(agent, content);
                if (unit != null)
                    queDialogue.Enqueue(unit);
            }
        }, 
        () => {
            init = true;
        }));
    }

    IEnumerator WaitForDosth(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
