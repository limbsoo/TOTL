using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;
using static PopupController;
using static UnityEngine.EventSystems.EventTrigger;

public class StageUI : UIEvent
{
    public static StageUI instance { get; private set; }
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
    }

    public Action OnDecideBlock;
    public Action OnInitializeUI;


    public Button _skill;
    Image _skillCoolDown;
    Image _SkillImage;
    public Slider stageTimeSlider;
    public GameObject fieldEffectPopUp;
    public GameObject damagedPopUp;

    void Start()
    {
        base.Start();
        ShowFieldEffectPopup();
        StageManager.instance.OnStageEnd += CompleteStage;

        StageManager.instance.OnPlayerEvent += OnPlayerEvent;

        //TextManager.instance.InitText();

        InitTexts();

        _skillCoolDown = _skill.gameObject.transform.GetChild(1).GetComponent<Image>();
        _SkillImage = _skill.gameObject.transform.GetChild(0).GetComponent<Image>();


        _SkillImage.sprite = Resources.Load<PlayerList>("ConstructSet/PlayerList").lists[DataManager.Instance.saveData.playerCharacterIdx].SkillSprite;


        StageManager.instance.OnPlayerDamaged += PlayerDamaged;
        StageManager.instance.OnPopUpOpen += OnPopUpOpen;
        StageManager.instance.OnEnemyCnt += OnEnemyCnt;
        StageManager.instance.OnUpadateText += UpdateText;
    }


    void Update()
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
            if (StageTimeSliderCoroutine != null)
            {
                StopCoroutine(StageTimeSliderCoroutine);
                stageTimeSlider.value = 0;
                StageTimeSliderCoroutine = null;
            }

        }
    }

    public TMP_Text curStage;
    public TMP_Text waveTimer;
    public TMP_Text curScore;
    public TMP_Text health;
    public TMP_Text gold;
    public TMP_Text EnemyCnt;
    public TMP_Text EnforcedEnemyCnt;

    void InitTexts()
    {
        curStage.text = string.Format("Wave {0}", DataManager.Instance.saveData.curWave.ToString());
        waveTimer.text = "0";
        curScore.text = string.Format("{0} / {1}", StageManager.instance.currentScore.ToString(), StageManager.instance.ReturnTargetScore().ToString());
        health.text = DataManager.Instance.saveData.playerStats.health.ToString();
        gold.text = DataManager.Instance.saveData.gold.ToString();
        EnemyCnt.text = string.Format("X {0}", "0");
        EnforcedEnemyCnt.text = string.Format("X {0}", "0");
    }

    public void OnEnemyCnt(float enemy, float enforcedEnemy)
    {
        EnemyCnt.text = string.Format("X {0}", enemy.ToString());
        EnforcedEnemyCnt.text = string.Format("X {0}", enforcedEnemy.ToString());
    }


    void OnPlayerEvent(PlayerEvent playerEvent, float value)
    {
        switch (playerEvent)
        {
            case PlayerEvent.Damaged:
                UpdateText("health", value);
                OnPopUpOpen(PopupType.GameOver);
                StartCoroutine(Function.instance.CountDown(0.5f, () => {
                    damagedPopUp.SetActive(false);
                }));

                break;

            case PlayerEvent.GetGold:
                break;

            case PlayerEvent.GetKey:
                break;

            case PlayerEvent.ArriveGoal:
                break;

            case PlayerEvent.SkillCoolDownUpdate:
                break;

            case PlayerEvent.Die:
                break;
        }
    }


    public void UpdateText(string s, float f)
    {
        switch (s) 
        {
            case "curStage":
                curStage.text = string.Format("Wave {0}", f.ToString());
                break;
            case "curScore":
                curScore.text = string.Format("{0} / {1}",f.ToString(), StageManager.instance.ReturnTargetScore().ToString());
                break;
            case "health":
                health.text = f.ToString();
                break;
            case "gold":
                gold.text = f.ToString();
                break;
        }

    }


    void OnPopUpOpen(PopupType popupType)
    {
        _popups[popupType].SetActive(true);
    }

    void PlayerDamaged(float f)
    {
        UpdateText("health", f);

        damagedPopUp.SetActive(true);

        StartCoroutine(Function.instance.CountDown(0.5f, () => {
            damagedPopUp.SetActive(false);
        }));
    }



    public void CompleteStage()
    {
        IdleSkillButton(true);

        fieldEffectPopUp.SetActive(true);
        OnInitializeUI?.Invoke();
    }



    public Action OnUseSkill;



    public void UseSkill()
    {

        OnUseSkill?.Invoke();
    }



    private void ShowFieldEffectPopup()
    {
        fieldEffectPopUp.SetActive(true);
    }

    public void DecideBlock()
    {
        fieldEffectPopUp.SetActive(false);

        DataManager.Instance.UpdateBlockData();


        OnDecideBlock?.Invoke();
    }




    public Coroutine SkillCoolDownCoroutine;
    public float MaxCoolDown;

    public void IdleSkillButton(bool CanUseSkill)
    {
        _skill.interactable = CanUseSkill;
    }

    public bool CheckCoolDown(float duration)
    {
        if (SkillCoolDownCoroutine != null)
        {
            if ((1 - _skillCoolDown.fillAmount) * MaxCoolDown < duration)
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

    //플레이어 이벤트
    public void StartCooldown(float duration)
    {

        if (SkillCoolDownCoroutine != null)
        {
            if((1 - _skillCoolDown.fillAmount) * MaxCoolDown < duration)
            {
                StopCoroutine(SkillCoolDownCoroutine);
                SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
            }
        }

        else
        {
            MaxCoolDown = duration;
            SkillCoolDownCoroutine = StartCoroutine(CooldownCoroutine(duration));
        }
    }


    public IEnumerator CooldownCoroutine(float duration)
    {
        IdleSkillButton(false);

        _skillCoolDown.gameObject.SetActive(true);

        _skillCoolDown.fillAmount = 1;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            //skillCoolDown.fillAmount = (1.0f / duration - elapsed);
            _skillCoolDown.fillAmount = 1 - Mathf.Clamp01(elapsed / duration);

            yield return null;
        }
        _skillCoolDown.gameObject.SetActive(false);

        MaxCoolDown = 0;

        IdleSkillButton(true);
        SkillCoolDownCoroutine = null;
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
                waveTimer.text = (StageManager.instance.waveTime).ToString();
            }
            
            yield return null;
        }
        stageTimeSlider.value = 0;
        StageManager.instance.waveTime = 0;
        StageTimeSliderCoroutine = null;

        StageManager.instance.lifeCycle++;
    }



    Coroutine StageTimeSliderCoroutine = null;



}



