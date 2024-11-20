using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    public Vector3 playerCameraDistance;

    static public Vector3 Vector3;
    public float smoothSpeed = 0.125f;

    public float speed = 10.0f;
    public Transform cameraTarget;

    private Camera thisCamera;
    private Vector3 worldDefalutForward;

    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        thisCamera = GetComponent<Camera>();
        worldDefalutForward = transform.forward;
    }

    public void Update()
    {
        rb = GetComponent<Rigidbody>();
        thisCamera = GetComponent<Camera>();
        worldDefalutForward = transform.forward;



        float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

        //this.transform.position = new Vector3(Vector3.x, Vector3.y + 50, Vector3.z - 30);
        //this.transform.position = new Vector3(Vector3.x, Vector3.y + 60, Vector3.z - 35);
        this.transform.position = new Vector3(Vector3.x - playerCameraDistance.x, Vector3.y - playerCameraDistance.y, Vector3.z - playerCameraDistance.z); 

        //if (thisCamera.fieldOfView <= 20.0f && scroll < 0) thisCamera.fieldOfView = 20.0f; //�ִ� ����
        //else if (thisCamera.fieldOfView >= 60.0f && scroll > 0) thisCamera.fieldOfView = 60.0f; // �ִ� �� �ƿ�
        //else thisCamera.fieldOfView += scroll; // ���� �ƿ� �ϱ�.


        // ���� ���� ������ ���� ĳ���͸� �ٶ󺸵��� �Ѵ�.
        if (cameraTarget && thisCamera.fieldOfView <= 30.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation
                , Quaternion.LookRotation(cameraTarget.position - transform.position)
                , 0.15f);
        }
        // ���� ���� �ۿ����� ������ ī�޶� �������� �ǵ��� ����.
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation
                , Quaternion.LookRotation(worldDefalutForward)
                , 0.15f);
        }
    }

    void LateUpdate()
    {
        //transform.position = new Vector3(Vector3.x, Vector3.y + 50, Vector3.z - 30);
        transform.position = new Vector3(Vector3.x, Vector3.y + 65, Vector3.z - 30);
    }

}
  




























    //public class CameraController : MonoBehaviour
    //{
    //    public float cameraSpeed;

    //    public GameObject player;

    //    private void Awake()
    //    {
    //        if (Player.camearInstance == null) Player.camearInstance = this;
    //    }


    //    private void Update()
    //    {


    //    }

    //    public void moveCamera(Vector3 player)
    //    {
    //        //player.z += 10;
    //        player.y += 25;

    //        //player.y *= Time.deltaTime;

    //        player.z -= 5;
    //        //player.z *= Time.deltaTime;
    //        this.transform.position = player;

    //        //�ε巴�� �߰��ϱ�


    //        //Vector3 dir = player - this.transform.position;
    //        //Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
    //        //this.transform.Translate(moveVector);
    //    }

    //}
//}

