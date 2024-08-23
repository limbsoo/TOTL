using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusEffectBlock : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Image image;

    public Sprite[] idle;
    int state;
    int spriteIdx;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        state = 0;
        spriteIdx = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TriggerChangeBlockState()
    {
        if(state == 0)
        {
            image.sprite = idle[1];
            //spriteRenderer.sprite = idle[1];
            state = 1;
        }

        else
        {
            image.sprite = idle[0];
            //spriteRenderer.sprite = idle[0];
            state = 0;
        }



        //switch (state)
        //{
        //    case 0:
        //}
    }

}
