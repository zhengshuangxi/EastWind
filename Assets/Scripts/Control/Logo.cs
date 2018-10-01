using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Operation;

public class Logo : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForEndOfOperation(new Move(transform, new Vector3(0, 2.2f, -1.5f), 3f, Finish).Start());
    }

    void Finish()
    {
        SceneManager.LoadScene("Main");
    }
}
