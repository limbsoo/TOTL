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

    public Slider cooldownSlider; // ��Ÿ���� ǥ���� UI �����̴�

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

    private void OnCooldownFinished()
    {
        // ��Ÿ�� ���� �� UI���� �߰����� �۾��� ������ �� �ֽ��ϴ�.
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
            Time.timeScale = 0f; //���� ����
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
        Time.timeScale = 1f; // ���� ����

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
        // ���� ���� ȭ���� ǥ���ϴ� ����
        Debug.Log("Game Over!");
    }


    
}
