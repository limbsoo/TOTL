using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Prepartion : MonoBehaviour
{
    public float maxBurstSpeed;     // 최대 버스트 속도
    public float chargeRate;         // 충전 속도
    public float turnSpeed;          // 회전 속도

    private Rigidbody rb;
    private Vector3 movementDirection;
    private float chargeAmount = 0f;
    private bool isCharging = false;

    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        collisionLayer = LayerMask.GetMask("wall");
        ball = transform;
        cueDirection = transform.forward;
        initialRotation = transform.rotation;

        Color startColor = new Color(1f, 0f, 0f, 0.5f); // 빨간색, 50% 투명
        Color endColor = new Color(1f, 1f, 0f, 0.5f); // 노란색, 50% 투명
        lineRenderer.startColor = startColor; // 시작 색상
        lineRenderer.endColor = endColor; // 끝 색상

        //lineRenderer.startColor = Color.black; // 시작 색상
        //lineRenderer.endColor = Color.black; // 끝 색상

        lineRenderer.startWidth = 0.1f; // 시작 굵기
        lineRenderer.endWidth = 0.1f; // 끝 굵기
    }

    Projector projector;



    private Transform ball;
    public float maxForce = 10f;

    private Vector3 cueDirection;

    void Update()
    {
        HandleInput();

        //Vector3 cueDirection = transform.forward;  // 큐의 방향 (공이 바라보는 방향)

        //ball = this.transform;

        ball = transform;
        cueDirection = transform.forward;

        PredictTrajectory(ball.position, cueDirection);

    }

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

            projector = this.transform.GetChild(0).gameObject.GetComponent<Projector>();
            projector.orthographicSize += 3.2f * Time.deltaTime;
        }
        else
        {
            if (isCharging && chargeAmount > 0f)
            {
                // 키를 놓은 순간에 튀어나가게 함
                BurstForward();
                chargeAmount = 0f;
            }
            isCharging = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        //// 회전
        //if (movementDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        //    transform.rotation = targetRotation;
        //}

        //// 회전
        //if (movementDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        //}

        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    void BurstForward()
    {
        // Rigidbody에 순간적인 힘을 가함
        rb.AddForce(transform.forward * chargeAmount, ForceMode.VelocityChange);
    }


    public LineRenderer lineRenderer;
    public int maxReflections = 4;  // 최대 반사 횟수
    public float maxLength = 300f;  // 최대 궤적 길이
    public LayerMask collisionLayer;  // 충돌 레이어

    // 공의 초기 위치와 방향을 받아 궤적을 예측합니다.
    public void PredictTrajectory(Vector3 startPosition, Vector3 direction)
    {
        // Line Renderer 초기화
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPosition);

        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = direction.normalized;
        int reflections = 0;
        float remainingLength = maxLength;

        while (reflections < maxReflections)
        {
            Ray ray = new Ray(currentPosition, currentVelocity);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, remainingLength, collisionLayer))
            {
                Vector3 hitPoint = hit.point;
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitPoint);

                currentPosition = hitPoint;
                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal);

                remainingLength -= Vector3.Distance(ray.origin, hitPoint);
                reflections++;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + currentVelocity * remainingLength);
                break;
            }
        }
    }








}
