using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{

    public void loadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void loadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
