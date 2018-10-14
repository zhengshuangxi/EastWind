using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneChoice : UIScene {

    UISceneWidget[] widgets;
	protected override void Start () {
        base.Start();
        widgets = GetComponentsInChildren<UISceneWidget>();
        for (int i = 0; i < widgets.Length; i++)
        {
            widgets[i].OnMouseClick = OnChoiceImageClick;
        }
    }

    private void OnChoiceImageClick(UISceneWidget eventObj)
    {
        Debug.Log(eventObj.name);
    }
}
