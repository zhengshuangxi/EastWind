using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result
{
    public class EvalScore
    {
        public float total;
        public float fluency;
        public float integrity;
        public float accuracy;
    }

    public Error error= Error.NORMAL;
    public string content;
    public Dictionary<string, EvalScore> words = new Dictionary<string, EvalScore>();
    public EvalScore score=new EvalScore();
}
