using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        }
        else Destroy(gameObject);
    }



    public Action OnDecideBlock;
    public Action OnInitializeUI;


    public void LoadMainScene()
    {
        SceneManager.instance.LoadScene("MainScene");
        //GameManager.instance.loadMainScene();
    }

    //public Action OnLoadMainScene;

    //public static bool isReady = false;

    public Image skillCoolDown;

    public Button Skill;


    public Button DecideButton;

    public void IdleDecideButton(bool DidDecide)
    {
        DecideButton.interactable = DidDecide;
    }


    public void IdleSkillButton(bool CanUseSkill)
    {
        Skill.interactable = CanUseSkill;
    }


    public void CompleteStage()
    {
        fieldEffectPopUp.SetActive(true);
        OnInitializeUI?.Invoke();
    }



    public Image SealCoolDown;


    public Slider cooldownSlider;
    public GameObject fieldEffectPopUp;

    public Action OnUIisReady;


    public Slider stageTimeSlider;

    public Action OnUseSkill;


    public GameObject playerDamaged;

    public void UseSkill()
    {

        OnUseSkill?.Invoke();
    }


    private void Start()
    {
        ShowFieldEffectPopup();


        //StageManager.instance.stageLevelSetEnd += ShowFieldEffectPopup;
        StageManager.instance.OnStageEnd += CompleteStage;
        TextManager.instance.InitText();


        

        //isReady = true;
        //OnUIisReady.Invoke();
    }


    private void ShowFieldEffectPopup()
    {
        fieldEffectPopUp.SetActive(true);
    }

    public void DecideBlock()
    {
        fieldEffectPopUp.SetActive(false);
        OnDecideBlock?.Invoke();
    }



    Coroutine timerCoroutine = null;
    Coroutine StageTimerCoroutine = null;

    int time = 0;
    int stageTime = 0;











    public IEnumerator SEalCoroutine(float duration)
    {
        SealCoolDown.gameObject.SetActive(true);


        SealCoolDown.fillAmount = 1;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SealCoolDown.fillAmount = 1 - Mathf.Clamp01(elapsed / duration);

            yield return null;
        }



        SealCoolDown.gameObject.SetActive(false);

    }


    public Coroutine SkillCoolDownCoroutine;
    public float MaxCoolDown;


    public bool CheckCoolDown(float duration)
    {
        if (SkillCoolDownCoroutine != null)
        {
            if ((1 - skillCoolDown.fillAmount) * MaxCoolDown < duration)
            {
                StopCoroutine(SkillCoolDownCoroutine);
                SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
                return true;
            }


        }

        else
        {
            MaxCoolDown = duration;
            SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
        }

        return false;
    }




    //�÷��̾� �̺�Ʈ
    public void StartCooldown(float duration)
    {
        //if (StageManager.Sstate == StageState.Play)
        //{
        //    if (StageTimeSliderCoroutine == null)
        //    {
        //        StageTimeSliderCoroutine = StartCoroutine(stageTimer(10f));
        //    }
        //}

        //else
        //{
        //    if (StageTimeSliderCoroutine != null)
        //    {
        //        StopCoroutine(StageTimeSliderCoroutine);
        //        stageTimeSlider.value = 0;
        //        StageTimeSliderCoroutine = null;
        //    }


        //}



        if (SkillCoolDownCoroutine != null)
        {
            if((1 - skillCoolDown.fillAmount) * MaxCoolDown < duration)
            {
                StopCoroutine(SkillCoolDownCoroutine);
                SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
            }


            //SkillCoolDownCoroutine.Yield();


            
        }

        else
        {
            MaxCoolDown = duration;
            SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
        }



        //StartCoroutine(CooldownCoroutine(duration));
    }


    public IEnumerator CooldownCoroutine(float duration)
    {
        IdleSkillButton(false);

        skillCoolDown.gameObject.SetActive(true);

        skillCoolDown.fillAmount = 1;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            //skillCoolDown.fillAmount = (1.0f / duration - elapsed);
            skillCoolDown.fillAmount = 1 - Mathf.Clamp01(elapsed / duration);

            yield return null;
        }
        skillCoolDown.gameObject.SetActive(false);

        MaxCoolDown = 0;



        IdleSkillButton(true);

        SkillCoolDownCoroutine = null;






        //skillCoolDown.gameObject.SetActive(true);

        //skillCoolDown.fillAmount = 1;

        //float elapsed = 0f;
        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;
        //    skillCoolDown.fillAmount = (1.0f / duration - elapsed);

        //    skillCoolDown.fillAmount -= ()

        //    yield return null;
        //}
        //skillCoolDown.gameObject.SetActive(false);




        //cooldownSlider.gameObject.SetActive(true); // �����̴��� Ȱ��ȭ�մϴ�.
        //float elapsed = 0f;
        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;
        //    cooldownSlider.value = Mathf.Clamp01(elapsed / duration);
        //    yield return null;
        //}
        //cooldownSlider.value = 0;
        //cooldownSlider.gameObject.SetActive(false); // ��Ÿ���� ������ �����̴��� ��Ȱ��ȭ�մϴ�.

        //// ��Ÿ���� �������� �˸��� �̺�Ʈ�� �߻���ŵ�ϴ�.
        //EventManager.TriggerCooldownFinished();
    }


    private IEnumerator stageTimer(float duration)
    {
        float elapsed = 0f;

        float saved = 0;

        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            stageTimeSlider.value = Mathf.Clamp01(elapsed / duration);

            StageManager.instance.waveTime = (int)elapsed;

            if(saved + 1 < elapsed)
            {
                saved += 1;
                StageManager.instance.stageTime += 1;
            }


            
            yield return null;
        }
        stageTimeSlider.value = 0;
        StageManager.instance.waveTime = 0;
        StageTimeSliderCoroutine = null;

        StageManager.instance.lifeCycle++;
    }



    Coroutine StageTimeSliderCoroutine = null;


    private void Update()
    {
        if (StageManager.Sstate == StageState.Play)
        {
            if (StageTimeSliderCoroutine == null)
            {
                StageTimeSliderCoroutine = StartCoroutine(stageTimer(10f));
            }
        }

        else
        {
            if(StageTimeSliderCoroutine != null)
            {
                StopCoroutine(StageTimeSliderCoroutine);
                stageTimeSlider.value = 0;
                StageTimeSliderCoroutine = null;
            }


        }
        
    }


}






































//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//using UnityEngine.UI;

//public class UIManager : MonoBehaviour
//{
//    public static UIManager instance { get; private set; }
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            //DontDestroyOnLoad(gameObject);
//        }
//        else Destroy(gameObject);
//    }


//    //public Button DecideBlock { get; private set; }
//    //public Button CompleteStage { get; private set; }


//    public Action OnCompleteStage;



//    public void CompleteStage()
//    {
//        MapStatusPopUP.SetActive(true);
//        OnCompleteStage?.Invoke();
//    }





//    public TMP_Text countDown;
//    public TMP_Text timer;
//    public TMP_Text target;
//    public TMP_Text curScore;


//    public TMP_Text playerHealth;
//    public TMP_Text playerDamage;
//    public TMP_Text playerSpeed;
//    public TMP_Text playerCoolDown;

//    public TMP_Text playerPenealtyCoolDown;

//    public Slider cooldownSlider;

//    public TMP_Text gameEnd;




//    public TMP_Text curStage;
//    public TMP_Text waveClear;


//    public TMP_Text StageTimer;


//    public GameObject MapStatusPopUP;

//    //public TMP_Text score;

//    //public TMP_Text teleport;



//    //public Text newCountDown;




//    public GameObject StatusEffectPopUP;
//    public Action OnDecideBlock;

//    private void Start()
//    {
//        StageManager.instance.stageLevelSetEnd += ShowStatusEffectPopUP;














//        OnDecideBlock += InitializeStageTimer;
//        StageManager.instance.OnStageEnd += CompleteStage;

//        //GameData data = DataManager.Instance.data;

//        //playerHealth.text = "Health : " + data.health.ToString();
//        //playerDamage.text = "Damage : " + data.damamge.ToString();
//        //playerSpeed.text = "Speed : " + data.moveSpeed.ToString();
//        //playerCoolDown.text = "CoolDown : " + data.coolDown.ToString();
//        //playerPenealtyCoolDown.text = "Penealty : ";
//        //target.text = "Target : " + StageManager.LCS.targetScore.ToString();
//        //curStage.text = "CurStage : " + DataManager.Instance.data.curStage.ToString();


//        ////if (countDown != null)
//        ////{
//        ////    Time.timeScale = 0f; //���� ����
//        ////    StartCoroutine(StartGame());
//        ////}

//        //timer.text = "";
//        //StageTimer.text = "";
//        ////StartCoroutine(Timer());
//    }


//    private void ShowStatusEffectPopUP()
//    {
//        StatusEffectPopUP.SetActive(true);
//    }

//    public void DecideBlock()
//    {
//        StatusEffectPopUP.SetActive(false);
//        OnDecideBlock?.Invoke();
//    }



//    Coroutine timerCoroutine = null;
//    Coroutine StageTimerCoroutine = null;

//    int time = 0;
//    int stageTime = 0;

//    private void Update()
//    {
//        UpdateTextUIs();

//        if (timerCoroutine == null)
//        {
//            time++;
//            timerCoroutine = StartCoroutine(Timer(time));
//        }

//        if (StageTimerCoroutine == null)
//        {
//            stageTime++;
//            StageTimerCoroutine = StartCoroutine(StageTimeCheck(stageTime));
//        }

//    }

//    private void UpdateTextUIs()
//    {
//        playerHealth.text = "Health : " + Player.instance.health.ToString();
//        playerDamage.text = "Damage : " + Player.instance.damamge.ToString();
//        playerSpeed.text = "Speed : " + Player.instance.moveSpeed.ToString();
//        playerCoolDown.text = "CoolDown : " + Player.instance.coolDown.ToString();
//        playerPenealtyCoolDown.text = "Penealty : " + Player.instance.PenealtyTime.ToString();
//        curScore.text = "Score: " + StageManager.currentScore.ToString();
//        curStage.text = "CurStage : " + DataManager.Instance.data.curStage.ToString();
//    }


//    private IEnumerator Timer(int time)
//    {
//        yield return new WaitForSeconds(1);
//        timer.text = (time).ToString();
//        timerCoroutine = null;
//    }

//    private IEnumerator StageTimeCheck(int time)
//    {
//        yield return new WaitForSeconds(1);
//        StageTimer.text = (time).ToString();
//        StageTimerCoroutine = null;
//    }


//    public void InitializeStageTimer()
//    {
//        StopCoroutine(StageTimerCoroutine);
//        stageTime = 1;
//        //StageTimerCoroutine = null;
//        StageTimerCoroutine = StartCoroutine(StageTimeCheck(stageTime));
//    }



//    private IEnumerator StartGame()
//    {
//        timer.text = "";
//        //countDown.text = "3";

//        ////newCountDown.text = "2";


//        ////float startTime = Time.time;
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "2";
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "1";
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "GO!";
//        //yield return new WaitForSecondsRealtime(1);
//        countDown.gameObject.SetActive(false);
//        Time.timeScale = 1f; // ���� ����

//        for (int i = 0; i < 59; i++)
//        {
//            yield return new WaitForSeconds(1);
//            timer.text = (59 - i).ToString();
//        }

//        //float startTime = Time.realtimeSinceStartup;
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "2";
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "1";
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.text = "GO!";
//        //yield return new WaitForSecondsRealtime(1);
//        //countDown.gameObject.SetActive(false);
//        //Time.timeScale = 1f; // ���� ����

//        //for (int i = 0; i < 59; i++)
//        //{
//        //    yield return new WaitForSecondsRealtime(1);
//        //    timeLimit.text = (59 - i).ToString();
//        //}
//    }

//    //public void StartCount()
//    //{
//    //    if (countDown != null)
//    //    {
//    //        Time.timeScale = 0f; //���� ����
//    //        StartCoroutine(StartGame());
//    //    }
//    //}


//    public void stageClear()
//    {
//        UIManager.instance.waveClear.text = "Wave Clear";

//        StartCoroutine(Function.instance.CountDown(1, () => {
//            UIManager.instance.waveClear.text = "";
//        }));
//    }



//    private void OnEnable()
//    {
//        //EventManager.instance.OnLoadPreparationScene += StartCount;
//        //EventManager.instance.OnLoadPlayScene += StartCount;

//        EventManager.instance.OnMainStageReady += setMainUI;







//        //�÷��̾� �̺�Ʈ
//        //EventManager.OnSkillUsed += StartCooldown;
//        EventManager.OnCooldownFinished += OnCooldownFinished;


//        EventManager.instance.OnScoreChanged += UpdateScore;
//        EventManager.instance.OnPlayerDied += ShowGameOverScreen;
//        EventManager.instance.OnGameEnd += showGameEndScreen;
//        //EventManager.instance.OnUseTeleport += teleportCoolDown;


//    }

//    private void OnDisable()
//    {
//        //EventManager.instance.OnLoadPreparationScene -= StartCount;
//        //EventManager.instance.OnLoadPlayScene -= StartCount;

//        EventManager.instance.OnMainStageReady -= setMainUI;



//        //�÷��̾� �̺�Ʈ
//        //EventManager.OnSkillUsed -= StartCooldown;
//        EventManager.OnCooldownFinished -= OnCooldownFinished;



//        EventManager.instance.OnScoreChanged -= UpdateScore;
//        EventManager.instance.OnPlayerDied -= ShowGameOverScreen;
//        EventManager.instance.OnGameEnd -= showGameEndScreen;
//        //EventManager.instance.OnUseTeleport -= teleportCoolDown;


//    }


//    public void setMainUI()
//    {

//    }












//    //�÷��̾� �̺�Ʈ
//    public void StartCooldown(float duration)
//    {
//        StartCoroutine(CooldownCoroutine(duration));
//    }

//    private void OnCooldownFinished()
//    {
//        // ��Ÿ�� ���� �� UI���� �߰����� �۾��� ������ �� �ֽ��ϴ�.
//        Debug.Log("Cooldown finished, ready to use skill again.");
//    }

//    private IEnumerator CooldownCoroutine(float duration)
//    {
//        cooldownSlider.gameObject.SetActive(true); // �����̴��� Ȱ��ȭ�մϴ�.
//        float elapsed = 0f;
//        while (elapsed < duration)
//        {
//            elapsed += Time.deltaTime;
//            cooldownSlider.value = Mathf.Clamp01(elapsed / duration);
//            yield return null;
//        }
//        cooldownSlider.value = 0;
//        cooldownSlider.gameObject.SetActive(false); // ��Ÿ���� ������ �����̴��� ��Ȱ��ȭ�մϴ�.

//        // ��Ÿ���� �������� �˸��� �̺�Ʈ�� �߻���ŵ�ϴ�.
//        EventManager.TriggerCooldownFinished();
//    }





















//    public void teleportCoolDown()
//    {
//        //StartCoroutine(CoolDown());
//    }

//    //private IEnumerator CoolDown()
//    //{
//    //    yield return new WaitForSecondsRealtime(1);
//    //    teleport.text = "2";
//    //    yield return new WaitForSecondsRealtime(1);
//    //    teleport.text = "1";
//    //    yield return new WaitForSecondsRealtime(1);
//    //    teleport.text = "Ready";
//    //}


//    void showGameEndScreen()
//    {
//        //gameEnd.SetActive(true);
//    }

//    public void UpdateScore(int newScore)
//    {
//        if (curScore != null)
//        {
//            curScore.text = "Score: " + newScore;
//        }

//        //score.text = "Score : " + newScore;

//        //scoreText.text = "Score: " + newScore;
//    }

//    void ShowGameOverScreen()
//    {
//        // ���� ���� ȭ���� ǥ���ϴ� ����
//        Debug.Log("Game Over!");
//    }



//}
