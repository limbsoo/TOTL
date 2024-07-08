using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public TMP_Text score;
    public int cnt = 0;

    public TMP_Text countdownText;
    private float startTime;

    private void Awake()
    {
        if (GameSet.textInstance == null) GameSet.textInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (countdownText != null)
        {
            Time.timeScale = 0f; //게임 정지
            StartCoroutine(StartGame());
        }
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + cnt;
    }

    private IEnumerator StartGame()
    {
        countdownText.text = "3";
        startTime = Time.realtimeSinceStartup;
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1);
        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f; // 게임 시작
    }
}
