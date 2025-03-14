using UnityEngine;

public class Floater : MonoBehaviour
{
    public delegate void SurfacedBoat(float surfaced);
    public static event SurfacedBoat OnSurfacedBoat;
    
    public Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    // private Transform boatTransform;
    // Update is called once per frame
    void Start()
    {
        OnSurfacedBoat?.Invoke(depthBeforeSubmerged);
    }
    void FixedUpdate()
    {
        float waveHeight = depthBeforeSubmerged; // [TODO] replace with a grab on the mesh height + 15
        if (transform.position.y < waveHeight)
        {
            //do this while underwater
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.linearVelocity * waterDrag * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.deltaTime, ForceMode.VelocityChange);
        }else{
            // do this while above water
            float diff = 0.1f * (transform.position.y - waveHeight);
            rb.AddForceAtPosition((Physics.gravity / floaterCount) * diff, transform.position, ForceMode.Acceleration);
        }
    }
}
