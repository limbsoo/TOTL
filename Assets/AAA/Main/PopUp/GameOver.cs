using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : PopUp
{
    public TMP_Text topRate;

    void Start()
    {
        base.Start();
        topRate.text = string.Format("Top rate wave : {0}", DataManager.Instance.saveData.maxWave.ToString());
    }

    protected override void HandleButtonClicked(ButtonType buttonType, string name)
    {
        switch (buttonType)
        {
            case ButtonType.LoadMain:
                SceneManager.instance.LoadScene("MainScene");
                break;
        }
    }


    //private void OnEnable()
    //{

    //}


    //protected override void OnDestroy()
    //{
    //    foreach (var button in _buttons)
    //    {
    //        button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
    //    }

    //    //base.OnDisable();
    //}
}
