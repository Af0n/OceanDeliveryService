using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

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
        StartCoroutine(MovementLoop());
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            // Stop phase
            isMoving = false;
            anim.SetBool("Walking", false);
            agent.ResetPath();

            stopDuration = Random.Range(1f, 5f);
            yield return new WaitForSeconds(stopDuration);

            // Walk phase
            Vector3 destination = GetRandomNavMeshLocation(transform.position, 5f);
            if (destination != Vector3.zero)
            {
                agent.SetDestination(destination);
                isMoving = true;
                anim.SetBool("Walking", true);

                walkDuration = Random.Range(1f, 5f);
                yield return new WaitForSeconds(walkDuration);
            }
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
