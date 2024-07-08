using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public void changePlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void changeMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
