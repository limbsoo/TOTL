using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    //public Button DecideBlock { get; private set; }
    //public Button CompleteStage { get; private set; }

    public Action OnDecideBlock;
    public Action OnCompleteStage;

    public void DecideBlock()
    {
        OnDecideBlock?.Invoke();
    }

    public void CompleteStage()
    {
        OnCompleteStage?.Invoke();
    }





    public TMP_Text countDown;
    public TMP_Text timeLimit;
    public TMP_Text target;
    public TMP_Text curScore;


    public TMP_Text playerHealth;
    public TMP_Text playerDamage;
    public TMP_Text playerSpeed;
    public TMP_Text playerCoolDown;

    public TMP_Text playerPenealtyCoolDown;

    public Slider cooldownSlider;

    public TMP_Text gameEnd;




    public TMP_Text curStage;
    public TMP_Text waveClear;







    //public TMP_Text score;

    //public TMP_Text teleport;



    //public Text newCountDown;

    public void Start()
    {
        GameData data = DataManager.Instance.data;

        playerHealth.text = "Health : " + data.health.ToString();
        playerDamage.text = "Damage : " + data.damamge.ToString();
        playerSpeed.text = "Speed : " + data.moveSpeed.ToString();
        playerCoolDown.text = "CoolDown : " + data.coolDown.ToString();
        playerPenealtyCoolDown.text = "Penealty : ";
        target.text = "Target : " + StageManager.LCS.targetScore.ToString();
        curStage.text = "CurStage : " + DataManager.Instance.data.curStage.ToString();


        if (countDown != null)
        {
            Time.timeScale = 0f; //���� ����
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        timeLimit.text = "";
        //countDown.text = "3";

        ////newCountDown.text = "2";


        ////float startTime = Time.time;
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "2";
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "1";
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "GO!";
        //yield return new WaitForSecondsRealtime(1);
        countDown.gameObject.SetActive(false);
        Time.timeScale = 1f; // ���� ����

        for (int i = 0; i < 59; i++)
        {
            yield return new WaitForSeconds(1);
            timeLimit.text = (59 - i).ToString();
        }

        //float startTime = Time.realtimeSinceStartup;
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "2";
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "1";
        //yield return new WaitForSecondsRealtime(1);
        //countDown.text = "GO!";
        //yield return new WaitForSecondsRealtime(1);
        //countDown.gameObject.SetActive(false);
        //Time.timeScale = 1f; // ���� ����

        //for (int i = 0; i < 59; i++)
        //{
        //    yield return new WaitForSecondsRealtime(1);
        //    timeLimit.text = (59 - i).ToString();
        //}
    }

    public void StartCount()
    {
        if (countDown != null)
        {
            Time.timeScale = 0f; //���� ����
            StartCoroutine(StartGame());
        }
    }


    public void stageClear()
    {
        UIManager.instance.waveClear.text = "Wave Clear";

        StartCoroutine(Function.instance.CountDown(1, () => {
            UIManager.instance.waveClear.text = "";
        }));
    }



    private void OnEnable()
    {
        EventManager.instance.OnLoadPreparationScene += StartCount;
        EventManager.instance.OnLoadPlayScene += StartCount;

        EventManager.instance.OnMainStageReady += setMainUI;







        //�÷��̾� �̺�Ʈ
        //EventManager.OnSkillUsed += StartCooldown;
        EventManager.OnCooldownFinished += OnCooldownFinished;


        EventManager.instance.OnScoreChanged += UpdateScore;
        EventManager.instance.OnPlayerDied += ShowGameOverScreen;
        EventManager.instance.OnGameEnd += showGameEndScreen;
        //EventManager.instance.OnUseTeleport += teleportCoolDown;


    }

    private void OnDisable()
    {
        EventManager.instance.OnLoadPreparationScene -= StartCount;
        EventManager.instance.OnLoadPlayScene -= StartCount;

        EventManager.instance.OnMainStageReady -= setMainUI;



        //�÷��̾� �̺�Ʈ
        //EventManager.OnSkillUsed -= StartCooldown;
        EventManager.OnCooldownFinished -= OnCooldownFinished;



        EventManager.instance.OnScoreChanged -= UpdateScore;
        EventManager.instance.OnPlayerDied -= ShowGameOverScreen;
        EventManager.instance.OnGameEnd -= showGameEndScreen;
        //EventManager.instance.OnUseTeleport -= teleportCoolDown;


    }


    public void setMainUI()
    {

    }




    public void Update()
    {
        playerHealth.text = "Health : " + Player.instance.health.ToString();
        playerDamage.text = "Damage : " + Player.instance.damamge.ToString();
        playerSpeed.text = "Speed : " + Player.instance.moveSpeed.ToString();
        playerCoolDown.text = "CoolDown : " + Player.instance.coolDown.ToString();

        curScore.text = "Score: " + StageManager.currentScore.ToString();
        curStage.text = "CurStage : " + DataManager.Instance.data.curStage.ToString();
        playerPenealtyCoolDown.text = "Penealty : " + Player.instance.PenealtyTime.ToString();
    }









    //�÷��̾� �̺�Ʈ
    public void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
    }

    private void OnCooldownFinished()
    {
        // ��Ÿ�� ���� �� UI���� �߰����� �۾��� ������ �� �ֽ��ϴ�.
        Debug.Log("Cooldown finished, ready to use skill again.");
    }

    private IEnumerator CooldownCoroutine(float duration)
    {
        cooldownSlider.gameObject.SetActive(true); // �����̴��� Ȱ��ȭ�մϴ�.
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownSlider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        cooldownSlider.value = 0;
        cooldownSlider.gameObject.SetActive(false); // ��Ÿ���� ������ �����̴��� ��Ȱ��ȭ�մϴ�.

        // ��Ÿ���� �������� �˸��� �̺�Ʈ�� �߻���ŵ�ϴ�.
        EventManager.TriggerCooldownFinished();
    }





















    public void teleportCoolDown()
    {
        //StartCoroutine(CoolDown());
    }

    //private IEnumerator CoolDown()
    //{
    //    yield return new WaitForSecondsRealtime(1);
    //    teleport.text = "2";
    //    yield return new WaitForSecondsRealtime(1);
    //    teleport.text = "1";
    //    yield return new WaitForSecondsRealtime(1);
    //    teleport.text = "Ready";
    //}


    void showGameEndScreen()
    {
        //gameEnd.SetActive(true);
    }

    public void UpdateScore(int newScore)
    {
        if (curScore != null)
        {
            curScore.text = "Score: " + newScore;
        }

        //score.text = "Score : " + newScore;

        //scoreText.text = "Score: " + newScore;
    }

    void ShowGameOverScreen()
    {
        // ���� ���� ȭ���� ǥ���ϴ� ����
        Debug.Log("Game Over!");
    }


    
}
