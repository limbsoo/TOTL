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


    public void InitText()
    {
        curStage.text = DataManager.Instance.saveData.curWave.ToString();
        waveTimer.text = "";
        stageTimer.text = "";
        targetScore.text = "TargetScore : " + StageManager.instance.targetScore.ToString();
        curScore.text = StageManager.instance.currentScore.ToString() + " / " + StageManager.instance.targetScore.ToString();

        health.text = /*"Health : " +*/ DataManager.Instance.saveData.playerStats.health.ToString();
        //speed.text = "Speed : " + DataManager.Instance.saveData.playerStats.moveSpeed.ToString();
        //coolDown.text = "CoolDown : " + DataManager.Instance.saveData.playerStats.coolDown.ToString();
        //penealtyCoolDown.text = "Penealty : ";
        //nextGoals.text = "";

        gold.text = DataManager.Instance.saveData.gold.ToString();
    }

    public void UpdateTexts()
    {
        curStage.text = StageManager.instance.GetCurWave().ToString();
        curScore.text = StageManager.instance.currentScore.ToString() + " / " + StageManager.instance.targetScore.ToString();

        PlayerStats playerStats = StageManager.instance.GetPlayerStats();

        health.text = playerStats.health.ToString();
        speed.text = playerStats.moveSpeed.ToString();
        coolDown.text = playerStats.coolDown.ToString();

        gold.text = StageManager.instance.gold.ToString();
    }



    int waveTime = 0;
    int stageTime = 0;
    Coroutine waveTimeCoroutine = null;
    Coroutine stageTimeCoroutine = null;

    private void Update()
    {
        if (StageManager.Sstate == StageState.Play)
        {
            //curStage.text = StageManager.instance.GetCurWave().ToString();
            //curScore.text = StageManager.instance.currentScore.ToString() + " / " + StageManager.instance.targetScore.ToString();

            ////PlayerStats playerStats = StageManager.instance.GetPlayerStats();

            ////health.text = playerStats.health.ToString();



            ////speed.text = playerStats.moveSpeed.ToString();
            ////coolDown.text = playerStats.coolDown.ToString();
            ////penealtyCoolDown.text = "Penealty : " + StageManager.instance.p.PenealtyTime.ToString();


            waveTimer.text = (StageManager.instance.waveTime).ToString();
            stageTimer.text = ConvertTime(StageManager.instance.stageTime);


            //nextGoals.text = "";

            //for (int i = 0; i < StageManager.instance.goalList.Count; i++)
            //{
            //    nextGoals.text += StageManager.instance.goalList[i].ToString();
            //    nextGoals.text += " ";
            //}

            //gold.text = StageManager.instance.gold.ToString();

        }

    }


    public string ConvertTime(int time)
    {
        string s = "";

        int minute = time / 60;
        int second = time % 60;

        if (minute < 10) { s += "0"; }
        s += minute.ToString();

        s += " : ";

        if (second < 10) { s += "0"; }
        s += second.ToString();

        return s;
    }

}
