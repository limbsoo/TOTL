
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

// ����ȭ������ ���ƿ��� �� �ν��Ͻ��� �����ϴ� �޼���
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
//        float width = StageManager.mapTransform.localScale.x * 10f; // Unity �⺻ Plane�� ũ��� 10x10 ����
//        float height = StageManager.mapTransform.localScale.z * 10f;
//        newPos.x = Mathf.Clamp(newPos.x, StageManager.mapTransform.position.x - width / 2, StageManager.mapTransform.position.x + width / 2);
//        newPos.z = Mathf.Clamp(newPos.z, StageManager.mapTransform.position.z - height / 2, StageManager.mapTransform.position.z + height / 2);
//        rb.MovePosition(newPos); // ������ �̵� ó��
//        //rb.MoveRotation(lastRotation);

//        // �����̴� �������� ȸ��
//        //if (movement != Vector3.zero)
//        {
//            Quaternion newRotation = Quaternion.LookRotation(movement);
//            rb.MoveRotation(newRotation);

//        }

//        // �����̴� �������� ȸ��
//        //lastRotation = Quaternion.LookRotation(movement);


//    }

//    else
//    {
//        rb.velocity = Vector3.zero; // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
//        //rb.MoveRotation(lastRotation); // ������ ȸ�� �� ����
//    }

//    //rb.MoveRotation(lastRotation);

//    //else rb.velocity = Vector3.zero;  // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
//}