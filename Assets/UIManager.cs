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
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    public Slider cooldownSlider; // 쿨타임을 표시할 UI 슬라이더

    //private void OnEnable()
    //{
    //    EventManager.OnSkillUsed += StartCooldown;
    //    EventManager.OnCooldownFinished += OnCooldownFinished;
    //}

    //private void OnDisable()
    //{
    //    EventManager.OnSkillUsed -= StartCooldown;
    //    EventManager.OnCooldownFinished -= OnCooldownFinished;
    //}

    private void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
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

    private void OnCooldownFinished()
    {
        // 쿨타임 종료 후 UI에서 추가적인 작업을 수행할 수 있습니다.
        Debug.Log("Cooldown finished, ready to use skill again.");
    }













    //public Text scoreText;
    public TMP_Text timeLimit;
    public TMP_Text score;
    public TMP_Text countdownText;
    public TMP_Text teleport;

    public GameObject gameOver;


    void Start()
    {
        if (countdownText != null)
        {
            Time.timeScale = 0f; //게임 정지
            StartCoroutine(StartGame());
        }
    }


    //public GameObject mainButton;


    private IEnumerator StartGame()
    {
        timeLimit.text = "";
        countdownText.text = "3";
        float startTime = Time.realtimeSinceStartup;
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1);
        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f; // 게임 시작

        for(int i = 0; i < 59; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            timeLimit.text = (59 - i).ToString();
        }

        

    }


    private void OnEnable()
    {
        EventManager.instance.OnScoreChanged += UpdateScore;
        EventManager.instance.OnPlayerDied += ShowGameOverScreen;
        EventManager.instance.OnGameEnd += showGameEndScreen;
        EventManager.instance.OnUseTeleport += teleportCoolDown;
    }

    private void OnDisable()
    {
        EventManager.instance.OnScoreChanged -= UpdateScore;
        EventManager.instance.OnPlayerDied -= ShowGameOverScreen;
        EventManager.instance.OnGameEnd -= showGameEndScreen;
        EventManager.instance.OnUseTeleport -= teleportCoolDown;
    }

    public void teleportCoolDown()
    {
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSecondsRealtime(1);
        teleport.text = "2";
        yield return new WaitForSecondsRealtime(1);
        teleport.text = "1";
        yield return new WaitForSecondsRealtime(1);
        teleport.text = "Ready";
    }


    void showGameEndScreen()
    {
        gameOver.SetActive(true);
    }

    void UpdateScore(int newScore)
    {
        score.text = "Score : " + newScore;

        //scoreText.text = "Score: " + newScore;
    }

    void ShowGameOverScreen()
    {
        // 게임 오버 화면을 표시하는 로직
        Debug.Log("Game Over!");
    }


    
}
