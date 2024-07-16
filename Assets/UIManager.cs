using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }



    public Slider cooldownSlider; // ��Ÿ���� ǥ���� UI �����̴�

    //public Text scoreText;
    public TMP_Text timeLimit;
    public TMP_Text score;
    public TMP_Text countdownText;
    public TMP_Text teleport;

    public GameObject gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }


    private void OnEnable()
    {
        //�÷��̾� �̺�Ʈ
        EventManager.OnSkillUsed += StartCooldown;
        EventManager.OnCooldownFinished += OnCooldownFinished;


        EventManager.instance.OnScoreChanged += UpdateScore;
        EventManager.instance.OnPlayerDied += ShowGameOverScreen;
        EventManager.instance.OnGameEnd += showGameEndScreen;
        EventManager.instance.OnUseTeleport += teleportCoolDown;


    }

    private void OnDisable()
    {
        //�÷��̾� �̺�Ʈ
        EventManager.OnSkillUsed -= StartCooldown;
        EventManager.OnCooldownFinished -= OnCooldownFinished;



        EventManager.instance.OnScoreChanged -= UpdateScore;
        EventManager.instance.OnPlayerDied -= ShowGameOverScreen;
        EventManager.instance.OnGameEnd -= showGameEndScreen;
        EventManager.instance.OnUseTeleport -= teleportCoolDown;


    }


    //�÷��̾� �̺�Ʈ
    private void StartCooldown(float duration)
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
















    public void Start()
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

    public void UpdateScore(int newScore)
    {
        if (score != null)
        {
            score.text = "Score: " + newScore;
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
