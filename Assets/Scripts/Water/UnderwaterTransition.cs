using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Water
{
    public class UnderwaterTransition : MonoBehaviour
    {
        public float transitionSpeed = 2f;
        private float targetWeight = 0f;
        private float currentWeight = 0f;
        private Volume volume;

        private void Start()
        {
            volume = GetComponent<Volume>();
            volume.weight = 0f;
        }

        private void OnEnable()
        {
            UnderWaterTrigger.OnUnderWaterStateChange += HandleUnderWaterStateChange;
        }

        private void OnDisable()
        {
            UnderWaterTrigger.OnUnderWaterStateChange -= HandleUnderWaterStateChange;
        }

        private void HandleUnderWaterStateChange(bool isUnderwater)
        {
            targetWeight = isUnderwater ? 1f : 0f;
            StopAllCoroutines();
            StartCoroutine(FadeEffect());
        }

        private IEnumerator FadeEffect()
        {
            while (Mathf.Abs(currentWeight - targetWeight) > 0.01f)
            {
                currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);
                volume.weight = currentWeight;
                yield return null;
            }
            volume.weight = targetWeight;
        }
    }
}
