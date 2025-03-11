using System;
using UnityEngine;

namespace Water
{
    public class UnderWaterTrigger : MonoBehaviour
    {
        public static event Action<bool> OnUnderWaterStateChange;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("Entered Water Volume!");
            OnUnderWaterStateChange?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("Exited Water Volume");
            OnUnderWaterStateChange?.Invoke(false);
        }
    }
}
