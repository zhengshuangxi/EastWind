using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceStation : MonoBehaviour {

    private Transform cam;
    private string currChoose;//当前选择的物品
    private List<string> goods = new List<string> { "Wallet", "Passport", "Bag" };
    void Start () {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
        Audio.GetInstance().Play(AudioType.OPER, "missing");
	}

    Ray ray;
    RaycastHit hit;
    void Update () {
        ray = new Ray(cam.position, cam.forward);
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (goods.Contains(hit.collider.name))
                {
                    currChoose = hit.collider.name.ToLower();
                    PlayerPrefs.SetString("currChoose", currChoose);
                    SceneManager.LoadScene("PoliceOffice");
                }

            }
        }
    }
}
