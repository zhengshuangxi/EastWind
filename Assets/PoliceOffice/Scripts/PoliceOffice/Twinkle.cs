using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Twinkle : MonoBehaviour {

    public Ease ease;
    private SpriteRenderer sprite;
	void Start () {
        sprite = GetComponent<SpriteRenderer>();

        sprite.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
    }
	
}
