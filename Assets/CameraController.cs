using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;

    public GameObject player;

    private void Awake()
    {
        if (Player.camearInstance == null) Player.camearInstance = this;
    }


    private void Update()
    {


    }

    public void moveCamera(Vector3 player)
    {
        //player.z += 10;
        player.y += 25;

        //player.y *= Time.deltaTime;

        player.z -= 5;
        //player.z *= Time.deltaTime;
        this.transform.position = player;

        //부드럽게 추가하기


        //Vector3 dir = player - this.transform.position;
        //Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        //this.transform.Translate(moveVector);
    }

}