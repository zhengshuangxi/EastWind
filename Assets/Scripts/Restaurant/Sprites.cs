using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public Sprite[] allSprites;
    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    SpriteRenderer sr;

    private void Awake()
    {
        sr = transform.GetComponent<SpriteRenderer>();

        for (int i = 0; i < allSprites.Length; i++)
        {
            sprites[allSprites[i].name] = allSprites[i];
        }
    }

    public void Show(string content)
    {
        if (sprites.ContainsKey(content))
            sr.sprite = sprites[content];
        else
            sr.sprite = null;
    }
}
