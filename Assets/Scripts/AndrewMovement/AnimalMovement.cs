using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    public bool isLandAnimal;
    public float speed = 3.5f;

    public bool isMoving;
    private float walkDuration;
    private float stopDuration;

    [HideInInspector] public bool inWater;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.speed = speed;
        stopDuration = Random.Range(1f, 5f);
        walkDuration = Random.Range(1f, 5f);

        StartCoroutine(StopTimer());
    }

    void Update()
    {
        if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            isMoving = false;
            stopDuration = Random.Range(1f, 5f);
            StartCoroutine(StopTimer());
        }
    }

    IEnumerator StopTimer()
    {
        if (isLandAnimal)
        {
            anim.SetBool("Walking", false);
        }

        isMoving = false;
        agent.ResetPath();

        yield return new WaitForSeconds(stopDuration);

        walkDuration = Random.Range(1f, 5f);
        StartCoroutine(MoveTimer());
    }

    IEnumerator MoveTimer()
    {
        if (isLandAnimal)
        {
            anim.SetBool("Walking", true);
        }

        Vector3 destination = GetRandomNavMeshLocation(transform.position, 5f); // 5 = search radius
        if (destination != Vector3.zero)
        {
            agent.SetDestination(destination);
            isMoving = true;
        }

        yield return new WaitForSeconds(walkDuration);

        if (isMoving)
        {
            agent.ResetPath();
            isMoving = false;
            stopDuration = Random.Range(1f, 5f);
            StartCoroutine(StopTimer());
        }
    }

    Vector3 GetRandomNavMeshLocation(Vector3 origin, float distance)
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a valid point
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero; // Failed to find a point
    }
}
