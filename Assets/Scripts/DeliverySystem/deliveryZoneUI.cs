using UnityEngine;
public class DeliveryZoneUI : MonoBehaviour
{
    [SerializeField] private Canvas deliveryZoneCanvas;

    private void Awake()
    {
        if (deliveryZoneCanvas == null)
        {
            Debug.LogError("Delivery Zone Canvas is not assigned.");
        }
    }

    public void Show()
    {
        if (deliveryZoneCanvas != null)
        {
            deliveryZoneCanvas.enabled = true;
        }
    }

    public void Hide()
    {
        if (deliveryZoneCanvas != null)
        {
            deliveryZoneCanvas.enabled = false;
        }
    }
}