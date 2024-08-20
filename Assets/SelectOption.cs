using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectOption : MonoBehaviour
{
    public UnityEvent OnSelect;
    [SerializeField] Canvas canvas;
    public bool isSelect = false;

    CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void selecting()
    {
        if (!isSelect)
        {
            setGray();
            isSelect = true;
        }

        else
        {
            setOrigin();
            isSelect = false;
        }

        //BlockSelect,selectedOne();
    }

    public void setGray()
    {
        canvasGroup.alpha = .6f;
        //canvasGroup.blocksRaycasts = false;
    }

    public void setOrigin()
    {
        canvasGroup.alpha = 1f;
        //canvasGroup.blocksRaycasts = true;
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
