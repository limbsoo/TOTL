using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TextManager : MonoBehaviour
{
    public static TextManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }


    public TMP_Text curStage;
    public TMP_Text countDown;
    public TMP_Text waveTimer;
    public TMP_Text stageTimer;
    public TMP_Text targetScore;
    public TMP_Text curScore;

    public TMP_Text health;
    public TMP_Text damage;
    public TMP_Text speed;
    public TMP_Text coolDown;
    public TMP_Text penealtyCoolDown;

    public TMP_Text nextGoals;


    public TMP_Text gold;


    private void Start()
    {
    }


    public void InitText()
    {
        curStage.text = /*"CurWave : " + */DataManager.Instance.data.curWave.ToString();
        //countDown
        waveTimer.text = "";
        stageTimer.text = "";
        targetScore.text = "TargetScore : " + StageManager.instance.targetScore.ToString();
        curScore.text = "";
        health.text = /*"Health : " +*/ DataManager.Instance.data.health.ToString();
        damage.text = "Damage : " + DataManager.Instance.data.damamge.ToString();
        speed.text = "Speed : " + DataManager.Instance.data.moveSpeed.ToString();
        coolDown.text = "CoolDown : " + DataManager.Instance.data.coolDown.ToString();
        penealtyCoolDown.text = "Penealty : ";
        nextGoals.text = "";

        gold.text = "";
    }


    int waveTime = 0;
    int stageTime = 0;
    Coroutine waveTimeCoroutine = null;
    Coroutine stageTimeCoroutine = null;

    private void Update()
    {
        if(StageManager.Sstate == StageState.Play)
        {
            curScore.text = StageManager.instance.currentScore.ToString() + " / " + StageManager.instance.targetScore.ToString();

            health.text = /*"Health : " +*/ StageManager.instance.p.health.ToString();
            damage.text = "Damage : " + StageManager.instance.p.damamge.ToString();
            speed.text = "Speed : " + StageManager.instance.p.moveSpeed.ToString();
            coolDown.text = "CoolDown : " + StageManager.instance.p.coolDown.ToString();
            penealtyCoolDown.text = "Penealty : " + StageManager.instance.p.PenealtyTime.ToString();

            //health.text = /*"Health : " +*/ Player.instance.health.ToString();
            //damage.text = "Damage : " + Player.instance.damamge.ToString();
            //speed.text = "Speed : " + Player.instance.moveSpeed.ToString();
            //coolDown.text = "CoolDown : " + Player.instance.coolDown.ToString();
            //penealtyCoolDown.text = "Penealty : " + Player.instance.PenealtyTime.ToString();


            waveTimer.text = (StageManager.instance.waveTime).ToString();

            int minute = StageManager.instance.stageTime / 60;
            int second = StageManager.instance.stageTime % 60;

            string s = "";

            if (minute < 10) s += "0";
            s += minute.ToString();

            s += " : ";

            if (second < 10) s += "0";
            s += second.ToString();

            stageTimer.text = s;

            //stageTimer.text = (StageManager.instance.stageTime).ToString();






            nextGoals.text = "";

            for(int i = 0; i < StageManager.instance.goalList.Count;i++)
            {
                nextGoals.text += StageManager.instance.goalList[i].ToString();
                nextGoals.text += " ";
            }

            gold.text = StageManager.instance.p.gold.ToString();
            //gold.text = Player.instance.gold.ToString();


            //if (waveTimeCoroutine == null)
            //{
            //    waveTimeCoroutine = StartCoroutine(Function.instance.CountDown(1, () =>
            //    {
            //        waveTime++;
            //        waveTimer.text = (waveTime).ToString();
            //        waveTimeCoroutine = null;
            //    }));
            //}

            //if (stageTimeCoroutine == null)
            //{
            //    stageTimeCoroutine = StartCoroutine(Function.instance.CountDown(1, () =>
            //    {
            //        stageTime++;
            //        stageTimer.text = (stageTime).ToString();
            //        stageTimeCoroutine = null;
            //    }));
            //}
        }

        //else
        //{
        //    if(stageTimeCoroutine != null) StopCoroutine(stageTimeCoroutine);
        //    if(waveTimeCoroutine != null) StopCoroutine(waveTimeCoroutine);


        //}

    }

}
