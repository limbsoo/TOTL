using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{

    public void loadPlayScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
        EventManager.instance.TriggerOnGameReStart();
    }

    public void loadMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        
        //GameManager.instance.Start()
    }

    //prepartionScene
}
