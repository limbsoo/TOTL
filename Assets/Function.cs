using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Function : MonoBehaviour
{
    public static Function instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public IEnumerator CountDownPerTime(bool b, float f)
    {
        while(f > 0)
        {
            f--;
            yield return new WaitForSecondsRealtime(1);
        }

        b = !b;
    }

    public IEnumerator CountDown(float delay, Action act)
    {
        yield return new WaitForSeconds(delay);
        //yield return new WaitForSecondsRealtime(delay);
        act();
    }



    //internal IEnumerator CountDown(bool b, float f)
}
