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
        float waveHeight = depthBeforeSubmerged; // [TODO] replace with a grab on the mesh height + 15
        Vector3 gravity = Physics.gravity;
        Vector3 move = Vector3.zero;

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
        
            velocity.y += Mathf.Abs(gravity.y) * displacementMultiplier * Time.fixedDeltaTime;

            velocity *= Mathf.Pow(waterDrag, Time.fixedDeltaTime);
        }
        else
        {
            float diff = 0.1f * (transform.position.y - waveHeight);
            velocity += (gravity / floaterCount) * diff * Time.fixedDeltaTime;
        }

        // Apply movement
        characterController.Move(velocity * Time.fixedDeltaTime);
    }

}
