using UnityEngine;

public class PlayerFloater : MonoBehaviour
{
    public delegate void SurfacedPlayer(float surfaced);
    public static event SurfacedPlayer OnSurfacedPlayer;

    public CharacterController characterController;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    void Start()
    {
        OnSurfacedPlayer?.Invoke(depthBeforeSubmerged);
    }

    void FixedUpdate()
    {
        velocity += Physics.gravity / floaterCount * Time.deltaTime;
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
    
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            velocity.y += Mathf.Abs(Physics.gravity.y) * displacementMultiplier * Time.deltaTime;
        
            velocity *= (1 - waterDrag * Time.deltaTime);
            angularVelocity *= (1 - waterAngularDrag * Time.deltaTime);
        }
    
        characterController.Move(velocity * Time.deltaTime);
        transform.Rotate(angularVelocity * Time.deltaTime);
    }

}
