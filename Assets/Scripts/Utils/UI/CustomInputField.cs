using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomInputField : InputField
{
    public Action valueChange;
    public Action valueSummit;
}
