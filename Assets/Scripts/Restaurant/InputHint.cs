using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHint : MonoBehaviour
{
    public Sprite[] icons;

    SpriteRenderer sr;
    bool showHint = false;
    float time = 0;
    int index = 0;
    float interval = 0.2f;

    private void Awake()
    {
        sr = transform.GetComponent<SpriteRenderer>();
    }

    public void StartShow()
    {
        time = 0;
        showHint = true;
    }

    public void StopShow()
    {
        sr.sprite = icons[0];
        showHint = false;
    }

    void Update()
    {
        if (showHint)
        {
            time += Time.deltaTime;

            if (time >= interval)
            {
                time = time - interval;
                index += 1;
                sr.sprite = icons[index % icons.Length];
            }
        }
    }
}
