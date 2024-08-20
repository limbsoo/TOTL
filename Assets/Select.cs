using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Select : MonoBehaviour
{
    CanvasGroup canvasGroup;
    //[SerializeField] Canvas canvas;

    //public static Select instance { get; private set; }
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        canvasGroup = GetComponent<CanvasGroup>();
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else Destroy(gameObject);
    //}

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public bool isSelect = false;

    public void Update()
    {
        if (isSelect) setOrigin();
        else setGray();
    }

    public void setGray()
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void setOrigin()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
