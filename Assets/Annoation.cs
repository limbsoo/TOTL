
//StageManager.cs

//public static StageManager instance;
//public static StageManager Instance
//{
//    get
//    {
//        if (instance == null)
//        {
//            instance = FindObjectOfType<StageManager>();
//            if (instance == null)
//            {
//                GameObject singletonObject = new GameObject(typeof(StageManager).ToString());
//                instance = singletonObject.AddComponent<StageManager>();
//                DontDestroyOnLoad(singletonObject);
//            }
//        }
//        return instance;
//    }
//}

//private void Awake()
//{
//    if (instance == null)
//    {
//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }
//    else if (instance != this)
//    {
//        Destroy(gameObject);
//    }
//}

// 메인화면으로 돌아왔을 때 인스턴스를 제거하는 메서드
//public void ResetInstance()
//{
//    instance = null;
//    Destroy(gameObject);
//}

//Player.cs

//public void Move()
//{
//    if (movement != Vector3.zero)
//    {
//        Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
//        float width = StageManager.mapTransform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
//        float height = StageManager.mapTransform.localScale.z * 10f;
//        newPos.x = Mathf.Clamp(newPos.x, StageManager.mapTransform.position.x - width / 2, StageManager.mapTransform.position.x + width / 2);
//        newPos.z = Mathf.Clamp(newPos.z, StageManager.mapTransform.position.z - height / 2, StageManager.mapTransform.position.z + height / 2);
//        rb.MovePosition(newPos); // 물리적 이동 처리
//        //rb.MoveRotation(lastRotation);

//        // 움직이는 방향으로 회전
//        //if (movement != Vector3.zero)
//        {
//            Quaternion newRotation = Quaternion.LookRotation(movement);
//            rb.MoveRotation(newRotation);

//        }

//        // 움직이는 방향으로 회전
//        //lastRotation = Quaternion.LookRotation(movement);


//    }

//    else
//    {
//        rb.velocity = Vector3.zero; // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
//        //rb.MoveRotation(lastRotation); // 마지막 회전 값 유지
//    }

//    //rb.MoveRotation(lastRotation);

//    //else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
//}