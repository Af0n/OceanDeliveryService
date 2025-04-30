using UnityEngine;

public class UpdateMap : MonoBehaviour
{
    public Transform playerTransform;
    public RectTransform marker;
    public RectTransform mapImage;

    // World boundaries
    private float worldMinX = -400f;
    private float worldMinZ = -400f;
    private float worldSize = 1600f;

    void Update()
    {
        // Get player's position in world
        float playerX = playerTransform.position.x;
        float playerZ = playerTransform.position.z;

        // Normalize position (0 to 1)
        float normalizedX = (playerX - worldMinX) / worldSize;
        float normalizedY = (playerZ - worldMinZ) / worldSize;

        // Get map image size in pixels (UI units)
        float mapWidth = mapImage.rect.width;
        float mapHeight = mapImage.rect.height;

        // Calculate marker position
        float markerX = normalizedX * mapWidth;
        float markerY = normalizedY * mapHeight;

        // Update marker anchored position
        marker.anchoredPosition = new Vector2(markerX, markerY);
    }
}
