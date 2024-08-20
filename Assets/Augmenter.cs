using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Augmenter : MonoBehaviour
{
    //나중에 선택할수있게 해주는것는 나중에

    public int length = 3;
    public GameObject blink;

    RectTransform rectTransform;
    public Vector3[] gridCenters;

    public void Start()
    {

        gridCenters = new Vector3[length];
        StartCoroutine(CoWaitForPosition()); //코루틴 시작
    }


    IEnumerator CoWaitForPosition()
    {
        yield return new WaitForEndOfFrame();
        rectTransform = GetComponent<RectTransform>();


        //GridLayoutGroup glg = GetComponent<GridLayoutGroup>();

        for(int i = 0;  i< length; i++) 
        {
            RectTransform childRect = rectTransform.GetChild(i).GetComponent<RectTransform>();
            gridCenters[i] = childRect.anchoredPosition3D;
            GameObject go = Instantiate(blink, gridCenters[i], blink.transform.rotation);
        }

        

        //float width = rectTransform.sizeDelta.x;
        //float cellWidth = width / length;
        //float startX = rectTransform.position.x - width / 2f;

        //for (int i = 0; i < length; i++)
        //{
        //    float centerX = startX + (i * cellWidth) + (cellWidth / 2f);
        //    gridCenters[i] = new Vector3(centerX, rectTransform.position.y, rectTransform.position.z);
        //}
    }
}
