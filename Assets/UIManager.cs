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













    //public TMP_Text score;

    //public TMP_Text teleport;

    

    //public Text newCountDown;

    public void Start()
    {
        PlayerData playerData = GameManager.instance.playerData;

        playerHealth.text = "Health : " + playerData.health.ToString();
        playerDamage.text = "Damage : " + playerData.damamge.ToString();
        playerSpeed.text = "Speed : " + playerData.moveSpeed.ToString();
        playerCoolDown.text = "CoolDown : " + playerData.coolDown.ToString();
        playerPenealtyCoolDown.text = "Penealty : ";
        target.text = "Target : " + StageManager.LCS.targetScore.ToString();


        if (countDown != null)
        {
            Time.timeScale = 0f; //게임 정지
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        timeLimit.text = "";
        countDown.text = "3";

        //newCountDown.text = "2";

        float startTime = Time.realtimeSinceStartup;
        yield return new WaitForSecondsRealtime(1);
        countDown.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countDown.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countDown.text = "GO!";
        yield return new WaitForSecondsRealtime(1);
        countDown.gameObject.SetActive(false);
        Time.timeScale = 1f; // 게임 시작

        for (int i = 0; i < 59; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            timeLimit.text = (59 - i).ToString();
        }
    }

    public void StartCount()
    {
        if (countDown != null)
        {
            Time.timeScale = 0f; //게임 정지
            StartCoroutine(StartGame());
        }
    }



    private void OnEnable()
    {
        EventManager.instance.OnLoadPreparationScene += StartCount;
        EventManager.instance.OnLoadPlayScene += StartCount;

        EventManager.instance.OnMainStageReady += setMainUI;







        //플레이어 이벤트
        EventManager.OnSkillUsed += StartCooldown;
        EventManager.OnCooldownFinished += OnCooldownFinished;


        EventManager.instance.OnScoreChanged += UpdateScore;
        EventManager.instance.OnPlayerDied += ShowGameOverScreen;
        EventManager.instance.OnGameEnd += showGameEndScreen;
        EventManager.instance.OnUseTeleport += teleportCoolDown;


    }

    private void OnDisable()
    {
        EventManager.instance.OnLoadPreparationScene -= StartCount;
        EventManager.instance.OnLoadPlayScene -= StartCount;

        EventManager.instance.OnMainStageReady -= setMainUI;



        //플레이어 이벤트
        EventManager.OnSkillUsed -= StartCooldown;
        EventManager.OnCooldownFinished -= OnCooldownFinished;



        EventManager.instance.OnScoreChanged -= UpdateScore;
        EventManager.instance.OnPlayerDied -= ShowGameOverScreen;
        EventManager.instance.OnGameEnd -= showGameEndScreen;
        EventManager.instance.OnUseTeleport -= teleportCoolDown;


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

        playerPenealtyCoolDown.text = "Penealty : " + Player.instance.PenealtyTime.ToString();
    }









    //플레이어 이벤트
    private void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
    }

    private void OnCooldownFinished()
    {
        // 쿨타임 종료 후 UI에서 추가적인 작업을 수행할 수 있습니다.
        Debug.Log("Cooldown finished, ready to use skill again.");
    }

    private IEnumerator CooldownCoroutine(float duration)
    {
        cooldownSlider.gameObject.SetActive(true); // 슬라이더를 활성화합니다.
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownSlider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        cooldownSlider.value = 0;
        cooldownSlider.gameObject.SetActive(false); // 쿨타임이 끝나면 슬라이더를 비활성화합니다.

        // 쿨타임이 끝났음을 알리는 이벤트를 발생시킵니다.
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
        // 게임 오버 화면을 표시하는 로직
        Debug.Log("Game Over!");
    }


    
}
