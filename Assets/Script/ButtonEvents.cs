using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    public Action<int> OnCheckMoney;

    public void TriggerOnCheckMoney(int gold)
    {
        if (OnCheckMoney != null) { OnCheckMoney(gold); }
    }





}
