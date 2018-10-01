using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static string scene;

    //Image image;
    //Text text;
    AsyncOperation async;

    public void Awake()
    {
        //image = transform.Find("Panel/Image").GetComponent<Image>();
        //text = transform.Find("Panel/Text").GetComponent<Text>();
        //text.text = "0%";
    }

    int progress = 0;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(scene);
        yield return async;
    }

    private void Update()
    {
        //image.fillAmount = async.progress;
       // text.text = string.Format("{0}%", (int)(async.progress * 100));
    }
}
