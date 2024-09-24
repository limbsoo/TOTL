using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{

    public int length = 10;
    public bool haveBlock = false;

    public RectTransform rectTransform;
    public Vector3[] gridCenters;

    public int startCell;
    public int endCell;

    public string blockName = "";
    public int idx;
    public int blockIdx;

    public int savedIdx;

    public FieldEffectBlock feb;

    private void Start()
    {
        //GridLayout gridLayout = GetComponent<GridLayout>();
        blockIdx = -1;
        savedIdx = -1;
        gridCenters = new Vector3[length];
        StartCoroutine(CoWaitForPosition()); //코루틴 시작


    }

    IEnumerator CoWaitForPosition()
    {
        yield return new WaitForEndOfFrame();
        rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        float cellWidth = width / length;
        float startX = rectTransform.position.x - width / 2f;

        for (int i = 0; i < length; i++)
        {
            float centerX = startX + (i * cellWidth) + (cellWidth / 2f);
            gridCenters[i] = new Vector3(centerX, rectTransform.position.y, rectTransform.position.z);
        }
    }

}
