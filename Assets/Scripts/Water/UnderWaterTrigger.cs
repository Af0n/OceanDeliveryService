using System;
using UnityEngine;

namespace Water
{
    public class UnderWaterTrigger : MonoBehaviour
    {
        public static event Action<bool> OnUnderWaterStateChange;
        public static event Action<bool> OnUnderWaterSurfaceChange;

        //Checks if the players body and head enter the water separately.
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("Entered Water Volume!");
                OnUnderWaterStateChange?.Invoke(true);
            }

            if (other.CompareTag("PlayerHead"))
            {
                //Debug.Log("Head Submerged!");
                OnUnderWaterSurfaceChange?.Invoke(true);
            }
        }

        //Checks if the players body or head exit the water separately.
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerHead"))
            {
                //Debug.Log("Head Surfaced!");
                OnUnderWaterSurfaceChange?.Invoke(false);
            }

            if (other.CompareTag("Player"))
            {
                //Debug.Log("Exited Water Volume");
                OnUnderWaterStateChange?.Invoke(false);
            }
        }
    }
}
