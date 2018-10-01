using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float speed;
    Vector3 axis;
    List<Color> colorList = new List<Color>()
    {
        Color.green,
        Color.red,
        Color.cyan,
        Color.yellow,
        Color.magenta,
    };

    private void Awake()
    {
        axis = UnityEngine.Random.rotation.eulerAngles;
        speed = UnityEngine.Random.Range(0.5f,2f);

        Color color = colorList[UnityEngine.Random.Range(0, colorList.Count)];
        transform.parent.Find("Text").GetComponent<TextMesh>().color = color;
        color.a = 0.3f;
        GetComponent<MeshRenderer>().material.SetColor("_Color",color);
    }

    private void Update()
    {
        transform.RotateAround(axis, speed * Time.deltaTime);
    }
}
