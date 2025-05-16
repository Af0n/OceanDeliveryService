using UnityEngine;
using System.Collections;

public class AquaticMovement : MonoBehaviour
{
    private CharacterController cc;

    public float moveSpeed = 3.5f;

    public bool isMoving;
    private float walkDuration;
    private float stopDuration;

    private Vector3 moveDirection;
    public bool inWater;
    private float yVelocity = Physics.gravity.magnitude;

    public LayerMask waterLayer;      // Assign your Water layer in inspector
    public LayerMask standableLayer;  // Assign your Land layer in inspector
    public float sampleRadius = 10f;  // How far to search for water spots
    
    private Coroutine moveCoroutine;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        stopDuration = Random.Range(1f, 5f);
        walkDuration = Random.Range(1f, 5f);

        StartCoroutine(StopTimer());
    }

    void Update()
    {
        if (isMoving)
        {
            if (!inWater)
            {
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                moveCoroutine = StartCoroutine(MoveTimer());
            }
            cc.Move(moveDirection * moveSpeed * Time.deltaTime);
            Vector3 flatDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            if (flatDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        if (!inWater)
        {
            Gravity();
        }
    }

    IEnumerator StopTimer()
    {
        isMoving = false;
        moveDirection = Vector3.zero;

        stopDuration = Random.Range(1f, 3f);
        yield return new WaitForSeconds(stopDuration);

        StartCoroutine(MoveTimer());
    }

    IEnumerator MoveTimer()
    {
        isMoving = true;
        moveDirection = Vector3.zero;

        Vector3 origin = transform.position + Vector3.up * 0.5f; // Raise the ray origin a bit to avoid hitting the ground below

        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = Random.Range(-0.5f, 0.5f);
            Vector3 candidatePosition = transform.position + randomDirection * sampleRadius;

            // Check if the candidate position is in water
            bool isInWater = Physics.CheckSphere(candidatePosition, 0.5f, waterLayer);

            // Check if there's a clear path to it (no land in the way)
            bool blockedByLand = Physics.Raycast(origin, (candidatePosition - origin).normalized,
                                                  Vector3.Distance(origin, candidatePosition),
                                                  standableLayer);

            if (isInWater && !blockedByLand)
            {
                moveDirection = (candidatePosition - transform.position).normalized;
                break;
            }
        }

        // If a valid direction wasn't found
        if (moveDirection == Vector3.zero)
        {
            isMoving = false;
            yield return new WaitForSeconds(1f);
            StartCoroutine(MoveTimer());
            yield break;
        }

        walkDuration = Random.Range(1f, 5f);
        yield return new WaitForSeconds(walkDuration);

        StartCoroutine(StopTimer());
    }

    public void Gravity()
    {
        cc.Move(Time.deltaTime * yVelocity * Vector3.down);
    }
}
