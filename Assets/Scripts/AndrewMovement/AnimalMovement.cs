using System.Collections;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private CharacterController cc;
    private Animator anim;
    
    public bool isLandAnimal;
    public float speed;

    public bool isMoving;
    private float walkDistanceTimer;
    private float stopTimer;
    
    private Vector3 landDirection;
    private Vector3 waterDirection;
    private float yVelocity = Physics.gravity.magnitude;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        stopTimer = Random.Range(1, 5);
        walkDistanceTimer = Random.Range(1, 5);
        StartCoroutine(StopTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isLandAnimal)
            {
                cc.Move(landDirection * speed * Time.deltaTime);
            }
            else
            {
                cc.Move(waterDirection * speed * Time.deltaTime);
            }
            
        }
    }

    public IEnumerator StopTimer()
    {
        anim.SetBool("Walking", false);
        isMoving = false;
        yield return new WaitForSeconds(stopTimer);
        isMoving = true;
        walkDistanceTimer = Random.Range(1, 5);
        StartCoroutine(MoveTimer());
    }

    public IEnumerator MoveTimer()
    {
        anim.SetBool("Walking", true);
        if (isLandAnimal)
        {
            landDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            transform.rotation = Quaternion.LookRotation(landDirection);
        }
        else
        {
            waterDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            transform.rotation = Quaternion.LookRotation(waterDirection);
        }
        
        isMoving = true;
        yield return new WaitForSeconds(walkDistanceTimer);
        isMoving = false;
        stopTimer = Random.Range(1, 5);
        StartCoroutine(StopTimer());
    }

    public void Gravity()
    {
        cc.Move(Time.deltaTime * yVelocity * Vector3.up);
    }
}
