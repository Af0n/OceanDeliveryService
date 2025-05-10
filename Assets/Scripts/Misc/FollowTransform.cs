using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;

    private void Update() {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
}
