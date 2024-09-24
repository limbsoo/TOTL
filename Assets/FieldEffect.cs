using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEffect : MonoBehaviour
{

    //public static float minusDamage = 0.5f;


    //public static string naming;

    //LightCondition lcd = LightCondition.Blink;

    //public static int startTime;
    private int endTime;
    private int startTime;
    private int gridIdx;

    private GameObject EffectRange;

    public void Start()
    {
        //moving = Vector3.zero;

        EffectRange = transform.GetChild(0).gameObject;
    }


    private void Awake()
    {
        //naming = gameObject.transform.parent.name;

        ////naming = gameObject.name;

        //startTime = StageManager.instance.startTime;
        //endTime = StageManager.instance.endTime;

    }

    public void Init(int start, int end, int idx)
    {
        startTime = start;
        endTime = end;
        gridIdx = idx;
    }


    //private Vector3 moving;

    Coroutine FieldEffectCoroutine = null;


    void Update()
    {
        if(StageManager.Sstate == StageState.Play)
        {
            if(StageManager.instance.waveTime == startTime)
            {
                if (FieldEffectCoroutine == null)
                {
                    switch (gameObject.tag)
                    {
                        case ("Blink"):
                            FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));
                            break;

                        case ("Shadow"):
                            FieldEffectCoroutine = StartCoroutine(shadowEffect(startTime, endTime, gridIdx));
                            break;

                        case ("Swamp"):
                            FieldEffectCoroutine = StartCoroutine(swampEffect(startTime, endTime));
                            break;
                    }
                }
            }



        }



        //if (StageManager.instance.waveTime == startTime)
        //{
        //    if (FieldEffectCoroutine == null)
        //    {
        //        switch (gameObject.tag)
        //        {
        //            case ("Blink"):

        //                FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));

        //                //blinking();
        //                break;

        //            case ("Shadow"):

        //                FieldEffectCoroutine = StartCoroutine(shadowEffect(startTime, endTime));

        //                break;

        //            case ("Disable(Clone)"):
        //                break;
        //        }

        //    }


        //}

        //if (moving != Vector3.zero)
        //{
        //    GameObject go = transform.GetChild(0).gameObject;
        //    Vector3 newPos = go.transform.position + moving * Time.fixedDeltaTime;
        //    go.transform.position = newPos;
        //}




        //DetectSpotlightArea();
    }

    private IEnumerator shadowEffect(int start, int end, int idx)
    {

        //EffectRange.transform.position = new Vector3(randXPos, transform.position.y, randZPos);






        //moving = new Vector3(50, 0, 0);
        yield return new WaitForSeconds(end - start);
        FieldEffectCoroutine = null;



        //EffectRange.SetActive(true);

        //float radius = 10;
        //float randXPos = UnityEngine.Random.Range(- radius + StageManager.instance.gridCenters[idx].x, radius + StageManager.instance.gridCenters[idx].x);
        //float randZPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].z, radius + StageManager.instance.gridCenters[idx].z);

        //EffectRange.transform.position = new Vector3(randXPos,transform.position.y, randZPos);


        ////moving = new Vector3(50, 0, 0);
        //yield return new WaitForSeconds(end - start);
        //EffectRange.SetActive(false);
        //FieldEffectCoroutine = null;
    }



    private IEnumerator blinkEffect(int start, int end)
    {
        EffectRange.SetActive(true);
        yield return new WaitForSeconds(end - start);

        EffectRange.SetActive(false);
        FieldEffectCoroutine = null;
    }

    private IEnumerator swampEffect(int start, int end)
    {
        EffectRange.SetActive(true);
        yield return new WaitForSeconds(end - start);

        EffectRange.SetActive(false);
        FieldEffectCoroutine = null;
    }




    //Coroutine BlinkingCoroutine = null;

    //void blinking()
    //{
    //    if (BlinkingCoroutine == null)
    //    {
    //        transform.GetChild(0).gameObject.SetActive(false);
    //        BlinkingCoroutine = StartCoroutine(blinkingIe());
    //    }
    //}

    //private IEnumerator blinkingIe()
    //{
    //    yield return new WaitForSeconds(3f);
    //    transform.GetChild(0).gameObject.SetActive(true);
    //    yield return new WaitForSeconds(3f);
    //    BlinkingCoroutine = null;


    //    //yield return new WaitForSecondsRealtime(3f);
    //    //transform.GetChild(0).gameObject.SetActive(true);
    //    //yield return new WaitForSecondsRealtime(3f);
    //    //BlinkingCoroutine = null;
    //}
}
