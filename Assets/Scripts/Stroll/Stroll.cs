using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stroll : MonoBehaviour
{
    Transform cam;

    void Awake()
    {
        cam = GameObject.Find("Pvr_UnitySDK/Head").transform;
    }


    void Update()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.transform.name== "Restaurant")
                {
                    Loading.scene = "Restaurant";
                    SceneManager.LoadScene("Loading");
                }
                else if(hit.transform.name == "Shopping")
                {
                    Loading.scene = "Market";
                    SceneManager.LoadScene("Loading");
                }
            }
        }
    }
}
