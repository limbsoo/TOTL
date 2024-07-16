using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Prepartion : MonoBehaviour
{
    public float maxBurstSpeed;     // �ִ� ����Ʈ �ӵ�
    public float chargeRate;         // ���� �ӵ�
    public float turnSpeed;          // ȸ�� �ӵ�

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

        Color startColor = new Color(1f, 0f, 0f, 0.5f); // ������, 50% ����
        Color endColor = new Color(1f, 1f, 0f, 0.5f); // �����, 50% ����
        lineRenderer.startColor = startColor; // ���� ����
        lineRenderer.endColor = endColor; // �� ����

        //lineRenderer.startColor = Color.black; // ���� ����
        //lineRenderer.endColor = Color.black; // �� ����

        lineRenderer.startWidth = 0.1f; // ���� ����
        lineRenderer.endWidth = 0.1f; // �� ����
    }

    Projector projector;



    private Transform ball;
    public float maxForce = 10f;

    private Vector3 cueDirection;

    void Update()
    {
        HandleInput();

        //Vector3 cueDirection = transform.forward;  // ť�� ���� (���� �ٶ󺸴� ����)

        //ball = this.transform;

        ball = transform;
        cueDirection = transform.forward;

        PredictTrajectory(ball.position, cueDirection);

    }

    void HandleInput()
    {
        // ������ �Է� ó��
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // ���� ���� ����
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
                // Ű�� ���� ������ Ƣ����� ��
                BurstForward();
                chargeAmount = 0f;
            }
            isCharging = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        //// ȸ��
        //if (movementDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        //    transform.rotation = targetRotation;
        //}

        //// ȸ��
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
        // Rigidbody�� �������� ���� ����
        rb.AddForce(transform.forward * chargeAmount, ForceMode.VelocityChange);
    }


    public LineRenderer lineRenderer;
    public int maxReflections = 4;  // �ִ� �ݻ� Ƚ��
    public float maxLength = 300f;  // �ִ� ���� ����
    public LayerMask collisionLayer;  // �浹 ���̾�

    // ���� �ʱ� ��ġ�� ������ �޾� ������ �����մϴ�.
    public void PredictTrajectory(Vector3 startPosition, Vector3 direction)
    {
        // Line Renderer �ʱ�ȭ
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
