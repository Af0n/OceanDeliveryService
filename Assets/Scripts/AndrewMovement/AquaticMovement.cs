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

    // Start is called before the first frame update
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

        stopDuration = Random.Range(1f, 5f);
        yield return new WaitForSeconds(stopDuration);

        StartCoroutine(MoveTimer());
    }

    IEnumerator MoveTimer()
    {
        isMoving = true;
        
        moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f));

        walkDuration = Random.Range(1f, 5f);
        yield return new WaitForSeconds(walkDuration);

        StartCoroutine(StopTimer());
    }
    
    public void Gravity()
    {
        cc.Move(Time.deltaTime * yVelocity * Vector3.down);
    }
}