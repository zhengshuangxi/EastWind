using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlay
{
    IEnumerator Begin();

    void Process(Dialog dialog);

    void Hide();

    void Display(string itemName);

    void Transfer(int type);

    void Select(int id);

    void Finish(float score);

    void PlayAnimation(string anim);
}
