using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;


public class Prepartion : MonoBehaviour
{
    public static Prepartion instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }




    public float maxBurstSpeed;     // 최대 버스트 속도
    public float chargeRate;         // 충전 속도
    public float turnSpeed;          // 회전 속도

    private Rigidbody rb;
    private Vector3 movementDirection;
    private float chargeAmount = 0f;
    private bool isCharging = false;

    private Quaternion initialRotation;


    public static DecalProjector projector;

    //public static Projector projector;

    bool isShoot = false;

    private Transform ball;
    public float maxForce = 10f;

    private Vector3 cueDirection;





    public float maxWidth; // 데칼 프로젝터의 최대 너비
    public float minWidth; // 데칼 프로젝터의 최소 너비
    public float fadeSpeed; // 너비가 증가하는 속도
    private float currentWidth; // 현재 너비







    public LineRenderer lineRenderer;
    public int maxReflections = 4;  // 최대 반사 횟수
    public float maxLength = 300f;  // 최대 궤적 길이
    public LayerMask collisionLayer;  // 충돌 레이어

    // 공의 초기 위치와 방향을 받아 궤적을 예측합니다.


    private Vector3 initialScale; // 초기 스케일

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collisionLayer = LayerMask.GetMask("wall");
        ball = transform;
        cueDirection = transform.forward;
        initialRotation = transform.rotation;
        lineRenderer = StageManager.instance.preMap.GetComponentInChildren<LineRenderer>();
        //initialScale = projector.size;
    }


    void Update()
    {
        if (StageManager.instance.Sstate == StageState.Play) return;

        HandleInput();



        if (!isShoot)
        {
            DrawTrajectory();

            if(isShoot) StartMovement();
        }

        else
        {
            
            MoveAlongTrajectory();
        }

    }

    void DrawTrajectory()
    {
        Vector3 startPosition = ball.position;
        Vector3 direction = ball.forward;

        positions.Clear();
        positions.Add(startPosition);

        //List<Vector3> positions = new List<Vector3>();
        //positions.Add(startPosition);

        float remainingDistance = maxLength;

        for (int i = 0; i < maxReflections; i++)
        {
            if (remainingDistance <= 0)
                break;

            RaycastHit hit;
            if (Physics.Raycast(startPosition, direction, out hit, maxLength, collisionLayer))
            {
                positions.Add(hit.point);
                remainingDistance -= Vector3.Distance(startPosition, hit.point);
                direction = Vector3.Reflect(direction, hit.normal);
                startPosition = hit.point;
            }
            else
            {
                positions.Add(startPosition + direction * maxLength);
                break;
            }
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private List<Vector3> positions = new List<Vector3>();
    private int currentPointIndex = 0;
    private bool isMoving = false;
    public float speed;



    void StartMovement()
    {
        if (positions.Count > 1)
        {
            isMoving = true;
            currentPointIndex = 1; // 첫 번째 포인트는 현재 위치이므로 두 번째 포인트부터 이동 시작
        }
    }


    void MoveAlongTrajectory()
    {
        if (currentPointIndex >= positions.Count)
        {
            isMoving = false;
            return;
        }

        Vector3 targetPosition = positions[currentPointIndex];
        ball.position = Vector3.MoveTowards(ball.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(ball.position, targetPosition) < 1.5f)
        {
            currentPointIndex++;
        }
    }

    private float currentStretch = 1.0f;

    void HandleInput()
    {
        // 움직임 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // 충전 상태 관리
        if (Input.GetKey(KeyCode.Space))
        {
            isCharging = true;
            chargeAmount = Mathf.Min(maxBurstSpeed, chargeAmount + chargeRate * Time.deltaTime);


            this.transform.GetChild(0).gameObject.SetActive(true);
            projector = this.transform.GetChild(0).gameObject.GetComponent<DecalProjector>();

            currentWidth = projector.size.x;
            currentWidth += fadeSpeed * Time.deltaTime;
            currentWidth = Mathf.Clamp(currentWidth, minWidth, maxWidth); // 너비를 최소값과 최대값 사이로 제한합니다.
            UpdateDecalWidth(currentWidth);


            //projector.orthographicSize += 3.2f * Time.deltaTime;
        }

        else
        {
            if (isCharging && chargeAmount > 0f)
            {
                // 키를 놓은 순간에 튀어나가게 함
                //BurstForward();
                chargeAmount = 0f;
                //projector.orthographicSize = 1;
                isShoot = true;
                //StartMovement();
            }
            isCharging = false;


            //currentWidth = projector.size.x;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }


        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    void UpdateDecalWidth(float width)
    {
        Vector3 size = projector.size;
        size.x = width;
        projector.size = size;
    }



    private void OnEnable()
    {
        //EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        EventManager.instance.OnPrePlayerCollisionPreEnemy += destoryEnemy;
    }

    private void OnDisable()
    {
        //EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        EventManager.instance.OnPrePlayerCollisionPreEnemy -= destoryEnemy;
    }


    public void destoryEnemy(GameObject g)
    {
        Destroy(g.gameObject);
    }



}








































//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;


//public class Prepartion : MonoBehaviour
//{
//    public static Prepartion instance { get; private set; }
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else Destroy(gameObject);
//    }




//    public float maxBurstSpeed;     // 최대 버스트 속도
//    public float chargeRate;         // 충전 속도
//    public float turnSpeed;          // 회전 속도

//    private Rigidbody rb;
//    private Vector3 movementDirection;
//    private float chargeAmount = 0f;
//    private bool isCharging = false;

//    private Quaternion initialRotation;


//    public static Projector projector;

//    bool isShoot = false;

//    private Transform ball;
//    public float maxForce = 10f;

//    private Vector3 cueDirection;



//    public LineRenderer lineRenderer;
//    public int maxReflections = 4;  // 최대 반사 횟수
//    public float maxLength = 300f;  // 최대 궤적 길이
//    public LayerMask collisionLayer;  // 충돌 레이어

//    // 공의 초기 위치와 방향을 받아 궤적을 예측합니다.

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        collisionLayer = LayerMask.GetMask("wall");
//        ball = transform;
//        cueDirection = transform.forward;
//        initialRotation = transform.rotation;

//        lineRenderer = StageManager.instance.preMap.GetComponentInChildren<LineRenderer>();


//        //Color startColor = new Color(1f, 0f, 0f, 1f); // 빨간색, 50% 투명
//        //Color endColor = new Color(1f, 1f, 0f, 1f); // 노란색, 50% 투명
//        //lineRenderer.startColor = startColor; // 시작 색상
//        //lineRenderer.endColor = endColor; // 끝 색상

//        //lineRenderer.startColor = Color.black; // 시작 색상
//        //lineRenderer.endColor = Color.black; // 끝 색상

//        //lineRenderer.startWidth = 0.1f; // 시작 굵기
//        //lineRenderer.endWidth = 0.1f; // 끝 굵기
//    }


//    void Update()
//    {
//        if (StageManager.instance.Sstate == StageState.Play) return;




//        HandleInput();

//        //Vector3 cueDirection = transform.forward;  // 큐의 방향 (공이 바라보는 방향)

//        //ball = this.transform;

//        ball = transform;
//        cueDirection = transform.forward;

//        //if (!isShoot)
//        if (!isMoving)
//        {
//            //PredictTrajectory(ball.position, cueDirection);
//            DrawTrajectory();
//        }

//        if(isMoving)
//        {
//            MoveAlongTrajectory();
//        }


//    }

//    void DrawTrajectory()
//    {
//        Vector3 startPosition = ball.position;
//        Vector3 direction = ball.forward;

//        positions.Clear();
//        positions.Add(startPosition);

//        //List<Vector3> positions = new List<Vector3>();
//        //positions.Add(startPosition);

//        for (int i = 0; i < maxReflections; i++)
//        {
//            RaycastHit hit;
//            if (Physics.Raycast(startPosition, direction, out hit, maxLength, collisionLayer))
//            {
//                positions.Add(hit.point);
//                direction = Vector3.Reflect(direction, hit.normal);
//                startPosition = hit.point;
//            }
//            else
//            {
//                positions.Add(startPosition + direction * maxLength);
//                break;
//            }
//        }

//        lineRenderer.positionCount = positions.Count;
//        lineRenderer.SetPositions(positions.ToArray());
//    }

//    private List<Vector3> positions = new List<Vector3>();
//    private int currentPointIndex = 0;
//    private bool isMoving = false;
//    public float speed = 5f;



//    void StartMovement()
//    {
//        if (positions.Count > 1)
//        {
//            isMoving = true;
//            currentPointIndex = 1; // 첫 번째 포인트는 현재 위치이므로 두 번째 포인트부터 이동 시작
//        }
//    }


//    void MoveAlongTrajectory()
//    {
//        if (currentPointIndex >= positions.Count)
//        {
//            isMoving = false;
//            return;
//        }

//        Vector3 targetPosition = positions[currentPointIndex];
//        ball.position = Vector3.MoveTowards(ball.position, targetPosition, speed * Time.deltaTime);

//        if (Vector3.Distance(ball.position, targetPosition) < 0.01f)
//        {
//            currentPointIndex++;
//        }
//    }



//    void HandleInput()
//    {
//        // 움직임 입력 처리
//        float moveHorizontal = Input.GetAxis("Horizontal");
//        float moveVertical = Input.GetAxis("Vertical");

//        movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

//        // 충전 상태 관리
//        if (Input.GetKey(KeyCode.Space))
//        {
//            isCharging = true;
//            chargeAmount = Mathf.Min(maxBurstSpeed, chargeAmount + chargeRate * Time.deltaTime);


//            this.transform.GetChild(0).gameObject.SetActive(true);

//            projector = this.transform.GetChild(0).gameObject.GetComponent<Projector>();
//            projector.orthographicSize += 3.2f * Time.deltaTime;
//        }

//        else
//        {
//            if (isCharging && chargeAmount > 0f)
//            {
//                // 키를 놓은 순간에 튀어나가게 함
//                //BurstForward();
//                chargeAmount = 0f;
//                projector.orthographicSize = 1;
//                isShoot = true;
//            }
//            isCharging = false;

//            this.transform.GetChild(0).gameObject.SetActive(false);
//            StartMovement();
//        }













//        //else
//        //{
//        //    if (isCharging && chargeAmount > 0f)
//        //    {
//        //        // 키를 놓은 순간에 튀어나가게 함
//        //        BurstForward();
//        //        chargeAmount = 0f;
//        //        projector.orthographicSize = 1;
//        //        isShoot = true;
//        //    }
//        //    isCharging = false;

//        //    this.transform.GetChild(0).gameObject.SetActive(false);
//        //}

//        //// 회전
//        //if (movementDirection != Vector3.zero)
//        //{
//        //    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
//        //    transform.rotation = targetRotation;
//        //}

//        //// 회전
//        //if (movementDirection != Vector3.zero)
//        //{
//        //    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
//        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//        //}

//        if (movementDirection != Vector3.zero)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//        }
//    }

//    void BurstForward()
//    {
//        // Rigidbody에 순간적인 힘을 가함
//        rb.AddForce(transform.forward * chargeAmount, ForceMode.VelocityChange);
//    }

//    public void PredictTrajectory(Vector3 startPosition, Vector3 direction)
//    {
//        // Line Renderer 초기화
//        lineRenderer.positionCount = 1;
//        lineRenderer.SetPosition(0, startPosition);

//        Vector3 currentPosition = startPosition;
//        Vector3 currentVelocity = direction.normalized;
//        int reflections = 0;
//        float remainingLength = maxLength;

//        while (reflections < maxReflections)
//        {
//            Ray ray = new Ray(currentPosition, currentVelocity);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit, remainingLength, collisionLayer))
//            {
//                Vector3 hitPoint = hit.point;
//                lineRenderer.positionCount += 1;
//                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitPoint);

//                currentPosition = hitPoint;
//                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal);

//                remainingLength -= Vector3.Distance(ray.origin, hitPoint);
//                reflections++;
//            }
//            else
//            {
//                lineRenderer.positionCount += 1;
//                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + currentVelocity * remainingLength);
//                break;
//            }
//        }
//    }


//}
